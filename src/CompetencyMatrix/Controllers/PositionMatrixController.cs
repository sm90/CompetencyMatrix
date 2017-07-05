using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CompetencyMatrix.Infrastructure;
using CompetencyMatrix.Models;
using CompetencyMatrix.Services;
using CompetencyMatrix.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;


// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CompetencyMatrix.Controllers
{
    [Authorize]
    public class PositionMatrixController : AuthorizedControllerBase
    {
        private const string MATRIX_NAME_ALREADY_EXISTS = "Matrix name {0} already exists";

        private readonly IAuthorizationService _authService;

        private readonly IPositionMatrixInheritanceService _inheritanceService;
        private readonly IPositionMatrixService _service;
        private readonly ViewRender _viewRender;
        private readonly ILogger<PositionMatrixController> _logger;

        public PositionMatrixController(ICompetencyMatrixContext context, IAuthorizationService authService,
            IPositionMatrixInheritanceService inheritanceService, IPositionMatrixService service,
            IServerVariables serverVariables, ViewRender viewRender, ILogger<PositionMatrixController> logger): base(context, serverVariables)
        {
            _authService = authService;
            _inheritanceService = inheritanceService;
            _service = service;
            _viewRender = viewRender;
            _logger = logger;
        }

        #region Create/Delete Matrix

        [HttpPost]
        public IActionResult Create(PositionMatrixDetails matrix)
        {
            var isNameUnique = IsMatrixNameUnique(matrix.Name);
            if (!isNameUnique)
            {
                ModelState.AddModelError("Name", string.Format("Name {0} is not unique!", matrix.Name));
            }
            if (!ModelState.IsValid) return PartialView("EditorTemplates/PositionMatrixCreateForm", matrix);

            try
            {
                matrix.Owner = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var createdPositionMatrix = _service.Create(matrix);
                
                return Json(new
                {
                    createdSuccessfully = true,
                    id = createdPositionMatrix.Id
                });
            }
            catch (Exception exception)
            {
                _logger.LogError(LoggingEvents.PositionMatrixCreation, exception, exception.Message);
                ModelState.AddModelError(string.Empty, "Failed to create position matrix. See Log for reason.");

                return PartialView("EditorTemplates/PositionMatrixCreateForm", matrix);
            }
        }

        private bool IsMatrixNameUnique(string matrixName)
        {
            return DbContext.PositionMatrix.All(x => x.Name != matrixName);
        }

        public async Task<IActionResult> Delete(int positionMatrixId)
        {
            var matrix = _service.GetPositionMatrix(positionMatrixId);
            if (!await _authService.AuthorizeAsync(UserWithPermissions, matrix, Operations.Delete))
            {
                return new ChallengeResult();
            }

            _service.DeleteMatrix(positionMatrixId);
            return Ok();
        }

        public async Task<IActionResult> InheritanceManagement(int positionMatrixId)
        {
            var currentMatrix = _service.GetFullPositionMatrixbyId(positionMatrixId);
            if (!await _authService.AuthorizeAsync( UserWithPermissions, currentMatrix, Operations.Update))
            {
                return new ChallengeResult();
            }
            var matrixesCanBeAdded = _inheritanceService.GetPossibleParents(currentMatrix);

            var viewModel = new PositionMatrixInheritanceManagementViewModel
            {
                CurrentMatrix = PositionMatrixDetails.FromDbModel(
                    DbContext.PositionMatrix.Include(x => x.Owner).SingleOrDefault(x => x.Id == positionMatrixId)),
                MatrixesCanBeAddedToParent = PositionMatrixList.FromDbModel(matrixesCanBeAdded),
                ParentMatrixes = PositionMatrixList.FromDbModel(
                    currentMatrix.PositionMatrixInheritanceMatrix.Select(p => p.ParentMatrix).Where(x => !x.IsDeleted))
            };

            return PartialView("EditorTemplates/PositionMatrixInheritanceManagement", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SetParentMatrixes(PositionMatrixInheritanceManagementViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("EditorTemplates/PositionMatrixInheritanceManagement", viewModel);
            }
            var matrix = _service.GetPositionMatrix(viewModel.CurrentMatrix.Id);
            if (!await _authService.AuthorizeAsync( UserWithPermissions, matrix, Operations.Update))
            {
                return new ChallengeResult();
            }
            _service.SetParentMatrixes(viewModel);

            return Ok();
        }

        #endregion

        #region Edit Matrix
        public IActionResult GetPositionMatrixSkillsUI(int positionMatrixId)
        {
            var skills = _service.GetPositionMatrixSkills(positionMatrixId);

            InitSkillUIData();

            return PartialView("DisplayTemplates/PositionMatrixSkillsTree", skills);
        }

        public IActionResult GetPositionMatrixDetailsUI(int positionMatrixId)
        {
            var positionMatrix = _service.GetFullPositionMatrixbyId(positionMatrixId);

            var viewModel = PositionMatrixDetails.FromDbModel(positionMatrix);

            return PartialView("EditorTemplates/PositionMatrixDetails", viewModel);
        }


        public IActionResult GetPositionMatrixEditorUI(int id)
        {
            var skills = _service.GetPositionMatrixSkills(id);

            InitSkillUIData();

            return View("EditorTemplates/PositionMatrixEditorTree", skills);
        }

        public object PrepareMatrixSkillGroup(int id, int? parentId, int matrixId, SkillViewType type)
        {
            InitSkillUIData();

            var model = _service.GetMatrixSkillData(id, parentId, matrixId, type);

            return new
            {
                View = _viewRender.Render("PositionMatrix/EditorTemplates/PositionMatrixSkills", model.Skills, ViewData),
                Json = Json(model)
            };
        }

        [HttpPost]
        public IActionResult UpdateMatrixDetails(int id, string name, string description)
        {
            var matrix = DbContext.PositionMatrix.FirstOrDefault(m => m.Id == id);
            if (matrix != null)
            {
                matrix.Name = name;
                matrix.Description = description;

                DbContext.Entry(matrix).State = EntityState.Modified;
                DbContext.SaveChanges();

                return Json(new
                {
                    updatedSuccessfully = true,
                    id = id
                });
            }
            else
                return Json(new
                {
                    updatedSuccessfully = false,
                    id = id
                });
        }

        [HttpPost]
        public void Update(string positionMatrix)
        {
            var skills = JsonConvert.DeserializeObject<PositionMatrixSkills>(positionMatrix);
            _service.Update(skills);
        }

        [HttpGet]
        public IActionResult EditSkillGroup(ViewModels.PositionMatrixSkillGroup group)
        {
            if (group.State == EntityState.Added)
            {
                group.GroupTypeId = _service.GetDefaultSkillGroupType().Id;
            }

            return PartialView("EditorTemplates/PositionMatrixCreateGroup", group);
        }

        [HttpPost]
        public object UpdateSkillGroup(ViewModels.PositionMatrixSkillGroup model)
        {
            InitSkillUIData();

            return new
            {
                View = _viewRender.Render("PositionMatrix/EditorTemplates/PositionMatrixSkillGroup", model, ViewData),
                Json = Json(model)
            };
        }

        private void InitSkillUIData()
        {
            var types = new List<SelectListItem>();

            DbContext.SkillGroupType.ToList().ForEach(x =>
                types.Add(new SelectListItem { Text = x.Name, Value = x.Id.ToString() })
            );

            var evalModels = DbContext.SkillEvaluationModel
                .Include(x => x.SkillEvaluationModelLevel).ThenInclude(x => x.SkillLevelModel)
                .ToList();

            var levels = new Dictionary<int, List<SelectListItem>>();

            foreach (var model in evalModels)
            {
                if (model.Id > 0 && levels.ContainsKey(model.Id))
                {
                    continue;
                }

                var list = new List<SelectListItem>();
                levels.Add(model.Id, list);

                foreach (var level in model.SkillEvaluationModelLevel)
                {
                    list.Add(new SelectListItem()
                    {
                        Text = level.SkillLevelModel.Name,
                        Value = level.SkillLevelModel.Id.ToString()
                    });
                }
            }

            ViewBag.SkillGroupTypes = types;
            ViewBag.SkillLevels = levels;
        }

        #endregion

        [HttpGet]
        [Produces("text/csv")]
        public ActionResult ExportToExcel(int matrixPositionId)
        {
            var positionMatrix = DbContext.PositionMatrix
                .Include(pm => pm.PositionMatrixInheritanceMatrix)
                .Include(pm => pm.PositionMatrixInheritanceParentMatrix)
                .ToList()
                .Single(e => e.Id == matrixPositionId);

            var viewModel = PositionMatrixDetails.FromDbModel(positionMatrix);
            var con = JsonConvert.SerializeObject(viewModel);
            var model = new List<PositionMatrixExcelModel>
            {
                new PositionMatrixExcelModel
                {
                    Id = viewModel.Id,
                    Name = viewModel.Name,
                    Descritiption = viewModel.Description
                },
            };
            return Ok(model);
        }
    }
}
