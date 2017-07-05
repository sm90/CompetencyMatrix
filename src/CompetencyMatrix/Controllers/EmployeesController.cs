using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using CompetencyMatrix.Infrastructure;
using CompetencyMatrix.Models;
using CompetencyMatrix.Utility;
using Microsoft.AspNetCore.Mvc;
using CompetencyMatrix.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CompetencyMatrix.Services;

namespace CompetencyMatrix.Controllers
{
    /// <summary>
    /// Represent the network logics for Employees window
    /// </summary>
    [SessionExpireFilter]

    public class EmployeesController : CompetencyMatrixBaseController
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesController(CompetencyMatrixContext context, IServerVariables serverVariables, IEmployeeService employeeService) : base(context, serverVariables)
        {
            _employeeService = employeeService;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Employees");
        }

        /// <summary>
        /// For support url {controller}{action}
        /// </summary>
        /// <returns></returns>
        public IActionResult Employees()
        {
            return View("Index");
        }

        [Authorize(Policy = "SkillAccess")]
        public IActionResult EmployeeSkills()
        {
            var employee = DbContext.Employee.First(x => x.Id == ServerVariables.CurrentEditUserId);
            ViewData["Positions"] = DbContext.PositionMatrix.Where(x => !x.IsDeleted).ToList().OrderBy(x => x.Name);
            ViewData["Buttons"] = PrepareApproveSubmitButtons(employee.Id);

            return View("EditorTemplates/EmployeeSkillDetailPopup", EmployeeModel.FromDbModel(employee, ServerVariables.CurrentUserId));
        }

        #region CRUD

        private IEnumerable<EmployeeSkillModel> GetSkills(bool withLog, bool withStore = false, bool isdndsuppport = true)
        {
            if (withStore)
            {
                var obj = ServerVariables.CurrentUserSkills;
                if (obj != null)
                    return obj.Skills;
            }
            else
            {
                ServerVariables.CurrentUserSkills = null;
            }

            var employee = DbContext.Employee.Single(a => a.Id == ServerVariables.CurrentEditUserId);

            var useTransactionData = employee.Id == ServerVariables.CurrentUserId
                || employee.Manager == ServerVariables.CurrentUserId
                && (employee.ProfileStatus == EmplyeeProfileStatus.Open || employee.ProfileStatus == EmplyeeProfileStatus.Submitted);

            List<EmployeeMatrixSkill> employeeSkills;

            if (useTransactionData)
            {
                //For not commited tranactions show pending changes
                employeeSkills = _employeeService.GetTransactionSkills(employee.MatrixId);
            }
            else
            {
                employeeSkills = DbContext.EmployeeMatrixSkill
                    .Where(a => (a.MatrixId == employee.MatrixId))
                        .Include(ems => ems.SkillLevel)
                        .Include(a => a.Skill).ThenInclude(a => a.ChangeLog)
                    .ToList();

            }

            var levelModel = DbContext.SkillLevelModel.ToList();

            var model = withLog ?
                EmployeeSkillModel.FromDbModelWithLogData(employeeSkills, levelModel, employee.MatrixId) :
                EmployeeSkillModel.FromDbModel(employeeSkills, levelModel, employee.MatrixId);

            if (withStore)
                ServerVariables.CurrentUserSkills = new EmployeeSkillStorageModel { Skills = model.ToList(), EmployeeId = ServerVariables.CurrentEditUserId };

            return model.OrderByDescending(a => a.SkillId).ThenByDescending(a => a.LastUsed);
        }

        public IEnumerable<EmployeeSkillModel> GetSkillsRoot(int id, bool useStore = false, bool isdndsuppport = true, bool withLog = true)
        {
            return GetSkills(withLog, useStore, isdndsuppport);
        }

        public IEnumerable<EmployeeSkillModel> GetSkillsRootWithStore(int id)
        {
            return GetSkillsRoot(0, true, false, false);
        }

        public JsonResult Clear()
        {
            _employeeService.CancelEmployeeTransaction(ServerVariables.CurrentEditUserId);

            return Json(new { result = true });
        }

        #region Past project 19-10-2016 last edit 
        //TODO Need change all drop down to the simply text field
        private IEnumerable<EmployeePastProjectModel> GetPastProject()
        {
            var model = DbContext.EmployeePastProject.Where(a => a.EmployeeId == ServerVariables.CurrentEditUserId)
                .ToList();

            var result = new List<EmployeePastProjectModel>();

            model.ForEach(action =>
            {
                var lastDate = action.WorkPeriodEnd.HasValue ? action.WorkPeriodEnd.Value.ToString("yyyy-dd-MM") : string.Empty;

                EmployeePastProjectModel item = new EmployeePastProjectModel
                {
                    CompanyName = action.Company,
                    Role = action.Role,
                    Project = action.Project,
                    ProjectDescription = string.IsNullOrEmpty(action.Description)
                        ? string.Empty
                        : action.Description.Replace("\r\n", "<br>").Reduce(100),
                    DescriptionTooltip = string.IsNullOrEmpty(action.Description)
                        ? string.Empty
                        : action.Description.Replace("\r\n", "<br>"),
                    Team = string.IsNullOrEmpty(action.Team)
                        ? string.Empty
                        : action.Team.Replace("\r\n", "<br>"),
                    Technologies = string.IsNullOrEmpty(action.Technology)
                        ? string.Empty
                        : action.Technology.Replace("\r\n", "<br>"),
                    Tools = string.IsNullOrEmpty(action.Tool)
                        ? string.Empty
                        : action.Tool.Replace("\r\n", "<br>"),
                    WorkPeriod = !string.IsNullOrEmpty(lastDate) ?
                        string.Format($"{action.WorkPeriodStart.ToString("yyyy-dd-MM")} - {lastDate}") :
                        string.Format($"{action.WorkPeriodStart.ToString("yyyy-dd-MM")}"),
                    EmployeePastProjectId = action.Id
                };
                result.Add(item);
            });


            return result.OrderByDescending(a => a.WorkPeriod);
        }

        public IEnumerable<EmployeePastProjectModel> GetPastProjectRoot(int id)
        {
            return GetPastProject();
        }

        [HttpPost]
        public ActionResult SavePastProjectDetails(EmployeePastProjectDetailModel value)
        {
            if (ModelState.IsValid)
            {
                bool invalidOperation = false;
                try
                {
                    try
                    {
                        DbContext.Database.BeginTransaction();
                        if (value.Id != 0)
                        {

                            var old = DbContext.EmployeePastProject.SingleOrDefault(a => a.Id == value.Id);
                            old.Company = value.Company;
                            old.Description = value.ProjectDescription;
                            old.WorkPeriodStart = value.WorkPeriodStart;
                            old.WorkPeriodEnd = value.WorkPeriodEnd;
                            old.Project = value.Project;
                            old.Role = value.Role;
                            old.Tool = value.Tools;
                            old.Technology = value.Technologies;
                            old.Team = value.Team;
                            DbContext.EmployeePastProject.Update(old);
                        }
                        else
                        {
                            EmployeePastProject pp = new EmployeePastProject();
                            pp.Company = value.Company;
                            pp.Description = value.ProjectDescription;
                            pp.WorkPeriodStart = value.WorkPeriodStart;
                            pp.WorkPeriodEnd = value.WorkPeriodEnd;
                            pp.Project = value.Project;
                            pp.Role = value.Role;
                            pp.Tool = value.Tools;
                            pp.Technology = value.Technologies;
                            pp.Team = value.Team;
                            pp.EmployeeId = ServerVariables.CurrentEditUserId;
                            var entity = DbContext.EmployeePastProject.Add(pp);
                        }
                        DbContext.SaveChanges();
                        if (!invalidOperation)
                        {
                            DbContext.Database.CommitTransaction();
                        }
                    }
                    catch (Exception)
                    {
                        invalidOperation = true;
                        DbContext.Database.RollbackTransaction();
                    }
                }
                finally
                {

                }
                return new JsonResult(new { result = !invalidOperation });
            }
            return new JsonResult(new { result = false });
        }

        /// <summary>
        /// Call from header by press button Edit/Add last project
        /// </summary>
        /// <param name="pastProjectId"></param>
        /// <returns></returns>
        public IActionResult GetEmployeePastProjectDetailModel(int pastProjectId)
        {
            EmployeePastProjectDetailModel model = new EmployeePastProjectDetailModel();


            if (pastProjectId > 0)
            {

                EmployeePastProject data = DbContext.EmployeePastProject.SingleOrDefault(a => a.Id == pastProjectId);

                var lastDate = data.WorkPeriodEnd.HasValue ? data.WorkPeriodEnd.Value.ToString("yyyy-dd-MM") : string.Empty;
                if (data != null)
                {
                    model.EmployeeId = data.EmployeeId;
                    model.Id = pastProjectId;
                    model.Company = data.Company;
                    model.ProjectDescription = data.Description;
                    model.Project = data.Project;
                    model.Role = data.Role;
                    model.Team = data.Team;
                    model.Technologies = data.Technology;
                    model.Tools = data.Tool;
                    model.WorkPeriodStart = data.WorkPeriodStart;
                    model.WorkPeriodEnd = data.WorkPeriodEnd;
                    model.WorkPeriodStartIso = data.WorkPeriodStart.ToString("yyyy-MM-dd");
                    model.WorkPeriodEndIso = lastDate;
                }
            }
            else
            {
                model = EmployeePastProjectDetailModel.CreateDefault();
                model.EmployeeId = ServerVariables.CurrentEditUserId;
                model.WorkPeriodStartIso = model.WorkPeriodStart.ToString("yyyy-MM-dd");
                model.WorkPeriodEndIso = string.Empty;
            }
            return PartialView("EditorTemplates/EmployeePastProjectDetail", model);
        }

        #endregion 

        [HttpGet("api/[controller]")]
        public IEnumerable<EmployeeModel> Get()
        {
            return DbContext.Employee
                .Include(x => x.MatrixApproval).ToList()
                .Select(x => EmployeeModel.FromDbModel(x, ServerVariables.CurrentUserId)).ToList();
        }

        [HttpGet]
        public IEnumerable<Employee> GetEmployeesByOffice(string id)
        {
            var result = DbContext.Employee.Where(a => string.Compare(a.Office, id) == 0).ToList();
            return result;
        }

        [HttpGet]
        public IEnumerable<Employee> GetEmployeesByPosition(string id)
        {
            var result = DbContext.Employee.Where(a => a.Title.Contains(id)).ToList();
            return result;
        }

        [HttpGet("api/[controller]/root")]
        public IEnumerable<EmployeeModel> GetRoot(int id)
        {
            if (id == 0)
                return Get().OrderBy(a => a.Name);
            else
            {
                var result = Get().Where(a => a.Id == id || (a.Manager.HasValue && a.Manager.Value == id)).OrderBy(a => a.Name);
                return result;
            }
        }

        [HttpGet("api/[controller]/GetChangeLogRoot")]
        public IEnumerable<EmployeeChangeLogModel> GetChangeLogRoot(int id)
        {
            int matrixId = GetMatrixIdByUser(ServerVariables.CurrentEditUserId);

            var changeLogs = DbContext
                .ChangeLog
                .Where(a => a.MatrixId == matrixId)
                .Include(a => a.Skill)
                .Include(a => a.ByWhomNavigation)
                .OrderBy(a => a.Id)
                .ThenBy(a => a.When)
                .ToList();

            var skillLevel = DbContext.SkillLevelModel.Select(a => a);

            var result = EmployeeChangeLogModel.FromDbModel(changeLogs, skillLevel);

            return result;
        }

        [HttpGet("api/[controller]/{id}")]
        public EmployeeModel Get(int id)
        {
            return Get().Single(c => c.Id == id);
        }

        [HttpGet("api/[controller]/employeeDetailSkillRoot")]
        public IEnumerable<SkillCategory> GetEmployeeDetailSkillRoot(int id)
        {
            var categories = DbContext.SkillCategory.ToList();
            // ReSharper disable once UnusedVariable
            var skills = DbContext.Skill.ToList();

            foreach (var category in categories)
            {
            }
            return categories.Where(c => c.ParentId == null);
        }

        public IActionResult CheckOnAccessPersonalInformation(int id)
        {
            var employees = DbContext.Employee;
            var employee = employees.Single(a => a.Id == id);

            int currentUserId = ServerVariables.CurrentUserId;
            bool accessGarant = id == currentUserId;
            if (User.IsInRole("EM"))
            {
                while (employee.Manager.HasValue)
                {
                    if (employee.Manager.Value == currentUserId)
                    {
                        accessGarant = true;
                        break;
                    }
                    employee = employees.Single(a => a.Id == employee.Manager.Value);
                }
            }
            return Json(new { accessDenied = accessGarant || User.IsInRole("HR") || User.IsInRole("Admin") });
        }

        public bool CheckOnEditSkills(int id)
        {
            int currentUserId = ServerVariables.CurrentUserId;
            int currentEditUserId = ServerVariables.CurrentEditUserId;
            var employees = DbContext.Employee;
            var employee = employees.Single(a => a.Id == currentEditUserId);

            bool accessGarantOffice = currentEditUserId == currentUserId;
            // check on office
            if (User.IsInRole("HR"))
            {
                var officeCurrentUser = employees.SingleOrDefault(a => a.Id == currentUserId).Office;
                var officeEditUser = employees.SingleOrDefault(a => a.Id == currentEditUserId).Office;
                if (string.IsNullOrEmpty(officeCurrentUser) || string.IsNullOrEmpty(officeEditUser) ||
                    string.CompareOrdinal(officeCurrentUser, officeEditUser) == 0)
                    accessGarantOffice = false;
            }
            // check on subordinates

            bool accessGarantsubordinates = false;
            if (User.IsInRole("EM"))
            {
                while (employee.Manager.HasValue)
                {
                    if (employee.Manager.Value == currentUserId)
                    {
                        accessGarantsubordinates = true;
                        break;
                    }
                    employee = employees.Single(a => a.Id == employee.Manager.Value);
                }
            }

            bool access = User.IsInRole("Admin") || (currentEditUserId == currentUserId) || accessGarantOffice ||
                          accessGarantsubordinates;
            return access;
        }

        public IActionResult CheckOnEditSkillAction()
        {
            bool access = CheckOnEditSkills(ServerVariables.CurrentEditUserId);
            return Json(new { accessDenied = access });
        }

        #endregion
        public IActionResult GetEmployeeDetails(int id)
        {

            Contract.Requires(id > 0);
            //TODO:remove ServerVariables.CurrentEditUserId
            ServerVariables.CurrentEditUserId = id;
            var employees = DbContext.Employee;
            var employee = employees.Include(x => x.MatrixApproval).Single(a => a.Id == id);
            ServerVariables.CurrentUserPosition = employee.Title;
            if (employee.Manager.HasValue)
                employee.ManagerNavigation = DbContext.Employee.Single(a => a.Id == employee.Manager.Value);
            ViewData["IsAllowEdit"] = CheckOnEditSkills(employee.Id);
            ViewData["Buttons"] = PrepareApproveSubmitButtons(employee.Id);

            return View("Employee", EmployeeModel.FromDbModel(employee, ServerVariables.CurrentUserId));
        }

        public IActionResult GetEmployeeSkillDetailView(int employeeId)
        {
            var employee = DbContext.Employee.Single(a => a.Id == ServerVariables.CurrentEditUserId);

            ViewBag.Buttons = PrepareApproveSubmitButtons(ServerVariables.CurrentEditUserId); //employee.ProfileStatus == EmplyeeProfileStatus.Submitted;

            return PartialView("EmployeeDetailListSkillPartial", EmployeeModel.FromDbModel(employee, ServerVariables.CurrentUserId));
        }

        public IActionResult DeleteSkillFromEmployee(string data)
        {
            Contract.Ensures(!string.IsNullOrEmpty(data));
            return MakeJsonResult(() =>
            {
                if (string.IsNullOrEmpty(data))
                    throw new ArgumentNullException($"DeleteSkillFromEmployee get empty string.");
                var ids = data.Split(':');
                var storage = ServerVariables.CurrentUserSkills;

                if (storage == null)
                {
                    storage = _employeeService.GetEmployeeSkillStorageModel(ServerVariables.CurrentUserId);
                }

                var toDelete = ids.Select(int.Parse).ToList();
                var entities = storage.Skills.Where(a => toDelete.Contains(a.EmployeeSkillId)).ToList();
                foreach (var employeeSkillModel in entities)
                {
                    storage.Skills.Remove(employeeSkillModel);
                }
                ServerVariables.CurrentUserSkills = storage;
                Contract.Ensures(storage != null);
            });
        }

        /// <summary>
        /// Save refreshed informataion aboout employee skills to the DB
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns></returns>
        public JsonResult SaveEmployeeSkills(int employeeId, EmplyeeProfileStatus status)
        {
            return MakeJsonResult(() =>
            {

                _employeeService.SaveEmployee(employeeId, status);
            });
        }

        public IActionResult UpdateSkillEmployee(EmployeeSkillLevelModel model)
        {
            var storage = ServerVariables.CurrentUserSkills;

            var date = Convert.ToDateTime(model.LastUsedIso);
            //Check on exist
            return MakeJsonResult(() =>
            {
                if (!model.IsEdit)
                {
                    // add new record 
                    //TODO should check on exists skill / skill level
                    storage.Skills.Add(new EmployeeSkillModel
                    {
                        ChangeLogId = 0,
                        State = EntityState.Added,
                        // ID from EmployeeMatrixSkill 
                        EmployeeSkillId = -model.SkillId,
                        Office = string.Empty,
                        LastUsed = date,
                        LastUsedYear = date.Year.ToString(),
                        LevelName = DbContext.SkillLevelModel.Single(a => a.Id == model.LevelId).Name,
                        LevelId = model.LevelId,
                        SkillId = model.SkillId,
                        SkillName = model.SkillName
                    });
                }
                else
                {
                    //Edit node 
                    //Old node will be 
                    //Clone to new node and set up in new node old LevelId
                    //var log = DbContext.ChangeLog.Single(a => a.Id == model.ChangeLogId);
                    //var newLog = new ChangeLog();
                    //var log = storage.Single(a => a.ChangeLogId == model.ChangeLogId);
                    var skillData = storage.Skills.SingleOrDefault(a => a.SkillId == model.SkillId);
                    if (skillData == null)
                        throw new InvalidOperationException("Error in storage. Data lost.");

                    skillData.LastUsed = date;
                    skillData.LevelId = model.LevelId;
                    skillData.SkillName = model.SkillName;
                    skillData.State = EntityState.Modified;
                    skillData.LevelName = DbContext.SkillLevelModel.Single(a => a.Id == model.LevelId).Name;
                    skillData.LastUsedYear = date.Year.ToString();
                }
                ServerVariables.CurrentUserSkills = storage;
            });
            //return Json(new { result = true });
        }

        /// <summary>
        /// Call before show edit employee skill form.
        /// </summary>
        /// <param name="employeeId">employeeId may be deprecate </param> TODO maybe remove because should use ServerVariable
        /// <param name="skillId"></param>
        /// <param name="isEdit"></param>
        /// <param name="changeLogId"></param>
        /// <returns></returns>
        public ActionResult GetEmployeeSkillLevel(int employeeId, int skillId, bool isEdit = false, int changeLogId = 0)
        {
            if (isEdit == false)
            {
                var skillCategories = DbContext.SkillCategory.ToList();

                var skill = DbContext.Skill
                    .Include(s => s.Category).ThenInclude(c => c.Parent)
                    .Include(s => s.SkillCriteria).ThenInclude(sc => sc.SkillLevelCriteria)
                    .Include(s => s.EvaluationModel)
                    .ThenInclude(em => em.SkillEvaluationModelLevel)
                    .ThenInclude(eml => eml.SkillLevelModel)
                    .Single(s => s.Id == skillId);

                var evaluationModels = DbContext
                    .SkillEvaluationModel.Include(sem => sem.SkillEvaluationModelLevel)
                    .ThenInclude(seml => seml.SkillLevelModel)
                    .ToList()
                    .Select(SkillEvaluationModelViewModel.FromDbModel).ToList();

                List<SkillLevelModelViewModel> levels = new List<SkillLevelModelViewModel>();
                evaluationModels.ForEach(a =>
                {
                    if (a.Id == skill.EvaluationModel.Id)
                        levels = a.Levels;
                });

                var model = new EmployeeSkillLevelModel
                {
                    EmployeeId = ServerVariables.CurrentUserId,
                    SkillName = skill.Name,
                    SkillId = skillId,
                    LastUsedIso = DateTime.Now.ToString(("yyyy-MM-dd")),
                    Levels = new List<SelectListItem>()
                };
                levels?.ForEach(a =>
                {
                    model.Levels.Add(new SelectListItem { Text = a.Name, Value = a.Id.ToString() });
                });

                return PartialView("EditorTemplates/EmployeeEditSkillDetail", model);
            }
            else
            {
                var skillCategories = DbContext.SkillCategory.ToList();

                var skill = DbContext.Skill
                    .Include(s => s.Category).ThenInclude(c => c.Parent)
                    .Include(s => s.SkillCriteria).ThenInclude(sc => sc.SkillLevelCriteria)
                    .Include(s => s.EvaluationModel)
                    .ThenInclude(em => em.SkillEvaluationModelLevel)
                    .ThenInclude(eml => eml.SkillLevelModel)
                    .Single(s => s.Id == skillId);

                var evaluationModels = DbContext
                   .SkillEvaluationModel.Include(sem => sem.SkillEvaluationModelLevel)
                   .ThenInclude(seml => seml.SkillLevelModel)
                   .ToList()
                   .Select(SkillEvaluationModelViewModel.FromDbModel).ToList();

                List<SkillLevelModelViewModel> levels = new List<SkillLevelModelViewModel>();
                evaluationModels.ForEach(a =>
                {
                    if (a.Id == skill.EvaluationModel.Id)
                        levels = a.Levels;
                });

                var storeSkill = ServerVariables.CurrentUserSkills.Skills.SingleOrDefault(a => a.SkillId == skillId);

                var model = new EmployeeSkillLevelModel
                {
                    EmployeeId = ServerVariables.CurrentUserId,
                    SkillName = skill.Name,
                    SkillId = skillId,
                    LastUsedIso = storeSkill.LastUsed?.ToString(("yyyy-MM-dd")), //log.When.ToString(("yyyy-MM-dd")),
                    Levels = new List<SelectListItem>(),
                    LevelId = storeSkill.LevelId, //log.SkillLevelId,
                    IsEdit = true
                    //,ChangeLogId = changeLogId
                };
                levels?.ForEach(a =>
                {
                    model.Levels.Add(new SelectListItem { Text = a.Name, Value = a.Id.ToString() });
                });

                return PartialView("EditorTemplates/EmployeeEditSkillDetail", model);
            }
        }

        public ActionResult GetGapAnalysisPdf(int employeeId, int positionId)
        {
            var model = CalculateGapAnalys(employeeId, positionId);

            return View("GapAnalysisPdf", model);
        }
        public ActionResult GetGapAnalysis(int employeeId)
        {
            var positions = DbContext.PositionMatrix.Where(x => !x.IsDeleted).ToList();

            ViewBag.Positions = positions;

            var model = CalculateGapAnalys(employeeId, positions.First().Id);

            return PartialView("GapAnalysisPartial", model);
        }

        public ActionResult GetGapAnalys(int employeeId, int positionId)
        {
            var model = CalculateGapAnalys(employeeId, positionId);

            return PartialView("GapAnalysisTablePartial", model);
        }

        [HttpGet("api/[controller]/matrixSkills")]
        public IEnumerable<PositionMatrixSkill> GetMatrixSkills(int id)
        {
            var skills = DbContext.PositionMatrixSkill.Where(m => m.MatrixId == id).ToList();

            return skills;
        }

        public EmployeePositionSkillsGaps CalculateGapAnalys(int employeeId, int positionId)
        {
            Contract.Requires(employeeId > 0);
            Contract.Requires(positionId > 0);

            var employee = DbContext.Employee
                .Where(x => x.Id == employeeId)
                .Include(x => x.Matrix).ThenInclude(x => x.EmployeeMatrixSkill).ThenInclude(x => x.Skill)
                .Include(x => x.Matrix).ThenInclude(x => x.EmployeeMatrixSkill).ThenInclude(x => x.SkillLevel)
                .FirstOrDefault();

            var positionMatrix = DbContext.PositionMatrix
                .Include(pm => pm.PositionMatrixInheritanceMatrix)
                .Include(pm => pm.PositionMatrixInheritanceParentMatrix)
                .ToList()
                .Single(e => e.Id == positionId);

            var positionMatrixSkills = DbContext.PositionMatrixSkill
                .Include(pms => pms.SkillGroup)
                .ThenInclude(e => e.GroupType)
                .Include(pms => pms.Skill)
                .Include(pms => pms.SkillLevel).ToList();

            var result = EmployeePositionSkillsGaps.FromDbModel(positionMatrix, employee);

            //To fix circular references when serializing
            result.Groups.ForEach(g =>
            {
                foreach (var gap in g.Gaps)
                {
                    if (gap.EmployeeSkill != null)
                    {
                        gap.EmployeeSkill.Matrix = null;
                        gap.EmployeeSkill.Skill.EmployeeMatrixSkill = null;
                        gap.EmployeeSkill.Skill.PositionMatrixSkill = null;
                        gap.EmployeeSkill.SkillLevel.EmployeeMatrixSkill = null;
                        gap.EmployeeSkill.SkillLevel.PositionMatrixSkill = null;
                    }
                    if (gap.PositionSkill != null)
                    {
                        gap.PositionSkill.Matrix = null;
                        gap.PositionSkill.Skill.EmployeeMatrixSkill = null;
                        gap.PositionSkill.Skill.PositionMatrixSkill = null;
                        gap.PositionSkill.SkillLevel.EmployeeMatrixSkill = null;
                        gap.PositionSkill.SkillLevel.PositionMatrixSkill = null;
                        gap.PositionSkill.SkillGroup = null;
                    }
                }
            });
            result.Gaps.ForEach(gap =>
            {
                if (gap.EmployeeSkill != null)
                {
                    gap.EmployeeSkill.Matrix = null;
                    gap.EmployeeSkill.Skill.EmployeeMatrixSkill = null;
                    gap.EmployeeSkill.Skill.PositionMatrixSkill = null;
                    gap.EmployeeSkill.SkillLevel.EmployeeMatrixSkill = null;
                    gap.EmployeeSkill.SkillLevel.PositionMatrixSkill = null;
                }
                if (gap.PositionSkill != null)
                {
                    gap.PositionSkill.Matrix = null;
                    gap.PositionSkill.Skill.EmployeeMatrixSkill = null;
                    gap.PositionSkill.Skill.PositionMatrixSkill = null;
                    gap.PositionSkill.SkillLevel.EmployeeMatrixSkill = null;
                    gap.PositionSkill.SkillLevel.PositionMatrixSkill = null;
                    gap.PositionSkill.SkillGroup = null;
                }
            });

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            ViewBag.Json = json;

            return result;
        }

        [HttpGet]
        [Produces("text/pdf")]
        public ActionResult GapAnalyseExportToPdf(int employeeId, int positionId)
        {
            var sss = new List<PositionMatrixExcelModel>() { new PositionMatrixExcelModel()
            {
                Id = employeeId,
                Name = positionId.ToString(),
                Descritiption = "viewModel.Description"
            }};

            return Ok(sss);
        }

        private List<EmployeeButtonType> PrepareApproveSubmitButtons(int employeeId)
        {
            var editedEmployee = DbContext.Employee.Single(a => a.Id == employeeId);
            var currentEmployee = DbContext.Employee.Single(a => a.Id == ServerVariables.CurrentUserId);

            var buttons = new List<EmployeeButtonType>();

            if (User.IsInRole("Admin"))
            {
                if (editedEmployee.ProfileStatus == EmplyeeProfileStatus.Submitted)
                {
                    buttons.Add(EmployeeButtonType.Approve);
                    buttons.Add(EmployeeButtonType.Reject);

                    //Profile submitted hide all buttons for owner
                    return buttons;
                }
                else
                {
                    buttons.Add(EmployeeButtonType.Cancel);
                    buttons.Add(EmployeeButtonType.Edit);
                    buttons.Add(EmployeeButtonType.Save);
                    buttons.Add(EmployeeButtonType.Submit);
                }

            }
            else if (currentEmployee.Id == editedEmployee.Id)
            {
                if (editedEmployee.ProfileStatus == EmplyeeProfileStatus.Submitted)
                {
                    //Profile submitted hide all buttons for owner
                    return buttons;
                }
                else
                {
                    buttons.Add(EmployeeButtonType.Cancel);
                    buttons.Add(EmployeeButtonType.Edit);
                    buttons.Add(EmployeeButtonType.Save);
                    buttons.Add(EmployeeButtonType.Submit);
                }
            }
            //TODO: I believe we should add some additional user with similar privileges
            else if (currentEmployee.Id == editedEmployee.Manager)
            {
                if (editedEmployee.ProfileStatus == EmplyeeProfileStatus.Submitted)
                {
                    buttons.Add(EmployeeButtonType.Approve);
                    buttons.Add(EmployeeButtonType.Reject);
                }
                //else
                //{
                //    buttons.Add(EmployeeButtonType.Cancel);
                //    buttons.Add(EmployeeButtonType.Edit);
                //    buttons.Add(EmployeeButtonType.Submit);
                //}
            }

            return buttons;

        }

        public void ApproveEmployee(int id)
        {
            _employeeService.ApproveEmployee(id, ServerVariables.CurrentUserId);
        }

        public void RejectEmployee(int id)
        {
            _employeeService.RejectEmployee(id, ServerVariables.CurrentUserId);
        }

    }

}
