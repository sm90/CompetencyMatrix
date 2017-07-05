using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompetencyMatrix.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using CompetencyMatrix.Utility;
using CompetencyMatrix.ViewModels;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CompetencyMatrix.Controllers
{
    [Authorize]
    public class SkillController : Controller
	{
		private CompetencyMatrixContext dbContext;

		public SkillController(CompetencyMatrixContext context)
		{
			dbContext = context;
		}


		#region CRUD

		[HttpGet("api/[controller]")]
		public IEnumerable<Skill> Get()
		{
			return dbContext.Skill.ToList();
		}

		[HttpGet("api/[controller]/{id}")]
		public Skill Get(int id)
		{
			return dbContext.Skill.Single(s => s.Id == id);
		}

		[HttpPost("api/[controller]")]
		public Skill Post(Skill skill)
		{
            var exists = dbContext.Skill.Any(s => s.Id == skill.Id);
            return dbContext.Upsert(skill, !exists);
		}

        [HttpDelete("api/[controller]/{id}")]
        public JsonResult Delete(int id)
        {
            var skillToDelete = dbContext.Skill.Include(s => s.SkillCriteria).ThenInclude(c => c.SkillLevelCriteria).Single(s => s.Id == id);
            var existsInMatrix = dbContext.PositionMatrixSkill.Any(p => p.SkillId == skillToDelete.Id);
            var existsInEmployeeMatrixSkill = dbContext.EmployeeMatrixSkill.Any(emp => emp.SkillId == skillToDelete.Id);

            if (existsInMatrix || existsInEmployeeMatrixSkill)
            {
                var errorMessage = string.Empty;
                var usingItemsList = new List<string>();
                if(existsInEmployeeMatrixSkill)
                {
                    var usingEmpMatrices = dbContext.EmployeeMatrixSkill.Where(m => m.SkillId == skillToDelete.Id).ToList();
                    var usingEmployees = dbContext.Employee.Where(e => usingEmpMatrices.Any(m => e.MatrixId == m.MatrixId)).ToList();

                    errorMessage = "Skill can not be removed. It is used in the following employees profiles:";
                    usingItemsList = usingEmployees.Select(u => u.Name).ToList();
                }
                else
                {
                    var usingPositionMatrixSkills = dbContext.PositionMatrixSkill.Where(p => p.SkillId == skillToDelete.Id);
                    var usingPositionMatrices = dbContext.PositionMatrix.Where(p => usingPositionMatrixSkills.Any(pms => pms.MatrixId == p.Id)).ToList();

                    if (usingPositionMatrices.Any(p => !p.IsPublic))
                        errorMessage = "Skill can not be removed. It is used in the private matrix.";
                    else
                    {
                        errorMessage = "Skill can not be removed. It is used in the following position matrixes:";
                        usingItemsList = usingPositionMatrices.Where(u => u.IsPublic).Select(u => u.Name).ToList();
                    }
                }
                return Json(new
                {
                    deleted = false,
                    error = errorMessage,
                    usingItemsList = usingItemsList
                });
            }
            else
            {
                var skillCriterias = skillToDelete.SkillCriteria.ToList();

                skillCriterias.ForEach(c => dbContext.SkillLevelCriteria.RemoveRange(c.SkillLevelCriteria));
                dbContext.SkillCriteria.RemoveRange(skillCriterias);
                dbContext.Skill.Remove(skillToDelete);

                dbContext.SaveChanges(true);

                return Json( new { deleted = true });
            }
        }

		public ViewModels.SkillEditorViewModel SaveSkillDetails(ViewModels.SkillEditorViewModel value)
		{
			var skill = dbContext.Skill.Single(s => s.Id == value.Id);

			skill.Name = value.Name;
			skill.Description = value.Description;
			skill.TrainingMaterials = value.TrainingMaterials;
			skill.Questionarie = value.Questionarie;
			skill.EvaluationModelId = value.EvaluationModelId;
			var updated = dbContext.Update(skill);
			var affectedSkill = dbContext.SaveChanges(true);

			if (value.CriteriaMatrix != null)
			{
				var affectedMatrix = value.CriteriaMatrix.Save(dbContext);
			}

			return value;
		}

		public IActionResult GetSkillDetailsView(int id, bool isEditable = true)
        {
			var skillCategories = dbContext.SkillCategory.ToList();

			var skill = dbContext.Skill
				.Include(s => s.Category).ThenInclude(c => c.Parent)
				.Include(s => s.SkillCriteria).ThenInclude(sc => sc.SkillLevelCriteria)
				.Include(s => s.EvaluationModel).ThenInclude(em => em.SkillEvaluationModelLevel).ThenInclude(eml => eml.SkillLevelModel)
				.Single(s => s.Id == id);

			var skillViewModel = ViewModels.SkillEditorViewModel.FromDbModel(skill);

			var evaluationModels = dbContext
				.SkillEvaluationModel.Include(sem => sem.SkillEvaluationModelLevel).ThenInclude(seml => seml.SkillLevelModel)
				.ToList()
				.Select(
					sem => SkillEvaluationModelViewModel.FromDbModel(sem)).ToList();

			evaluationModels.Insert(0, new SkillEvaluationModelViewModel {Id = null, Name = "<No model attached>"});

			skillViewModel.EvaluationModels = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(evaluationModels, "Id", "DisplayName",
				skillViewModel.EvaluationModelId);
            skillViewModel.IsEditable = isEditable;


            return PartialView("SkillDetails", skillViewModel);
		}

		public ViewModels.SkillLevelCriteriaMatrix SaveSkillLevelCriteriaMatrix(ViewModels.SkillLevelCriteriaMatrix value)
		{
			var affectedMatrix = value.Save(dbContext);
			return value;
		}

		public IActionResult ChangeEvaluationModel(SkillEditorViewModel evaluationViewModel)
		{
			var skill = dbContext.Skill
				.Include(s => s.SkillCriteria).ThenInclude(sc => sc.SkillLevelCriteria)
				.Include(s => s.EvaluationModel).ThenInclude(em => em.SkillEvaluationModelLevel).ThenInclude(eml => eml.SkillLevelModel)
				.AsNoTracking()
				.Single(s => s.Id == evaluationViewModel.Id);

			skill.EvaluationModelId = evaluationViewModel.EvaluationModelId;
			skill.EvaluationModel =
				dbContext.SkillEvaluationModel.Include(sem => sem.SkillEvaluationModelLevel).ThenInclude(eml => eml.SkillLevelModel).SingleOrDefault(sem => sem.Id == evaluationViewModel.EvaluationModelId);

			var matrix = new SkillLevelCriteriaMatrix(skill);
            matrix.IsEditable = evaluationViewModel.IsEditable;

			var view = PartialView("EditorTemplates/SkillLevelCriteriaMatrix", matrix);
			view.ViewData.TemplateInfo.HtmlFieldPrefix = "CriteriaMatrix";

			return view;
		}

		public IActionResult GetSkillLevelCriteriaMatrixView(int skillId)
		{
			var skill = dbContext.Skill
				.Include(s => s.SkillCriteria).ThenInclude(sc => sc.SkillLevelCriteria)
				.Include(s => s.EvaluationModel).ThenInclude(em => em.SkillEvaluationModelLevel).ThenInclude(eml => eml.SkillLevelModel)
				.Single(s => s.Id == skillId);

			var matrix = new SkillLevelCriteriaMatrix(skill);

			var view = PartialView("EditorTemplates/SkillLevelCriteriaMatrix", matrix);
			view.ViewData.TemplateInfo.HtmlFieldPrefix = "CriteriaMatrix";

			return view;
		}

		public IActionResult AddSkillLevelCriteriaMatrixRow(ViewModels.SkillEditorViewModel matrixViewModel)
		{
			matrixViewModel.CriteriaMatrix.AddCriteria("New criteria");

			var view = PartialView("EditorTemplates/SkillLevelCriteriaMatrix", matrixViewModel.CriteriaMatrix);
			view.ViewData.TemplateInfo.HtmlFieldPrefix = "CriteriaMatrix";

			return view;
		}

		public IActionResult UpdateSkillLevelCriteriaMatrixModel(ViewModels.SkillEditorViewModel matrixViewModel)
		{
			var rebuiltMatrix = matrixViewModel.CriteriaMatrix.Rebuild(dbContext);
            rebuiltMatrix.IsEditable = matrixViewModel.CriteriaMatrix.IsEditable;

			var view = PartialView("EditorTemplates/SkillLevelCriteriaMatrix", rebuiltMatrix);
			view.ViewData.TemplateInfo.HtmlFieldPrefix = "CriteriaMatrix";

			ModelState.Clear();

			return view;
		}

		#endregion

		[HttpPost("api/[controller]/move")]
		public Skill ChangeParent([FromBody] Skill value)
		{
			var skill = dbContext.Skill.Single(c => c.Id == value.Id);
			skill.CategoryId = value.CategoryId;
			var updated = dbContext.Update(skill);
			var affected = dbContext.SaveChanges(true);
			return updated.Entity;
		}

		//Clone a skill by passed Id and stich it to the passed CategoryId
		[HttpPost("api/[controller]/clone")]
		public Skill Clone([FromBody] Skill value)
		{
			var skill = Get(value.Id);
			var newSkill = skill.Clone();
			newSkill.CategoryId = value.CategoryId;

			var added = dbContext.Add(newSkill);
			var affected = dbContext.SaveChanges(true);
			return added.Entity;
		}

		[HttpPost("api/[controller]/rename")]
		public Skill ChangeName(Skill value)
		{
			var skill = dbContext.Skill.Single(c => c.Id == value.Id);
			skill.Name = value.Name;

			var updated = dbContext.Update(skill);
			var affected = dbContext.SaveChanges(true);
			return updated.Entity;
		}

        public IEnumerable<Skill> GetSkillsByEvaluationModel(int modelId)
        {
            return dbContext.Skill.Where(x => x.EvaluationModelId == modelId);        
        }
	}
}
