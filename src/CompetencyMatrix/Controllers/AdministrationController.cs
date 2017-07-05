using System.Linq;
using CompetencyMatrix.Infrastructure;
using CompetencyMatrix.Models;
using CompetencyMatrix.Services;
using CompetencyMatrix.ViewModels;
using CompetencyMatrix.ViewModels.Administration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PagedList.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace CompetencyMatrix.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdministrationController : CompetencyMatrixBaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AdministrationController> _logger;
        private const int ITEMS_PER_PAGE = 25;
        private readonly ILogsReportService _logsReportService;
        private readonly ITemplateService _mailTemplateService;

        public AdministrationController(
            UserManager<ApplicationUser> userManager,
            ILogger<AdministrationController> logger,
            CompetencyMatrixContext context,
            IServerVariables serverVariables,
            ILogsReportService logsReportService,
            ITemplateService mailTemplateService) : base(context, serverVariables)
        {
            _userManager = userManager;
            _logger = logger;
            _logsReportService = logsReportService;
            _mailTemplateService = mailTemplateService;
        }

        [HttpGet]
        public IActionResult Users()
        {
            var users = DbContext.AspNetUsers.ToList();

            return View(users);
        }

        [HttpGet]
        public IActionResult MailTemplates()
        {
            var templates = _mailTemplateService.GetMailTemplates();

            return View(templates);
        }

        [HttpGet]
        public IActionResult UpdateTemplate(MailTemplateType type)
        {
            var template = _mailTemplateService.GetMailTemplate(type);

            return View(ViewModels.MailTemplate.FromDBModel(template));
        }

        [HttpPost]
        public IActionResult UpdateTemplate(CompetencyMatrix.ViewModels.MailTemplate template)
        {
            _mailTemplateService.UpdateTemplate(template);

            return RedirectToAction("MailTemplates");
        }

        [HttpGet]
        public IActionResult CreateUser()
        {
            var model = new EditUserViewModel();

            var employees = DbContext.Employee.ToList();
            var roles = DbContext.AspNetRoles.ToList();

            ViewBag.Employees = employees;
            ViewBag.Roles = roles;
            return PartialView("CreateUserPartial", model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(EditUserViewModel model)
        {
            var appUser = new ApplicationUser()
            {
                Email = model.Email,
                UserName = model.UserName,
                PhoneNumber = model.PhoneNumber
            };
            model.Password = model.Password ?? "";
            var result = await _userManager.CreateAsync(appUser, model.Password);
            if (result.Succeeded)
            {
                if (!string.IsNullOrWhiteSpace(model.RoleId))
                {
                    var role = DbContext.AspNetRoles.Single(r => r.Id == model.RoleId);
                    await _userManager.AddToRoleAsync(appUser, role.Name);
                }
                if (model.EmployeeId != null)
                {
                    var user = DbContext.AspNetUsers.Include(u => u.AspNetUserRoles).Single(u => u.Id == appUser.Id);
                    user.EmployeeId = model.EmployeeId;
                    DbContext.Entry(user).State = EntityState.Modified;
                    DbContext.SaveChanges();
                }
            }
            return Json(new
            {
                result = result.Succeeded,
                errors = result.Errors
            });
        }

        [HttpGet]
        public IActionResult EditUser(string id)
        {
            var user = DbContext.AspNetUsers.Single(u => u.Id == id);
            var employees = DbContext.Employee.ToList();
            var roles = DbContext.AspNetRoles.ToList();

            var userRole = DbContext.AspNetUserRoles.FirstOrDefault(r => r.UserId == user.Id);

            var model = EditUserViewModel.FromDbModel(user);
            if (userRole != null)
                model.RoleId = userRole.RoleId;

            ViewBag.Employees = employees;
            ViewBag.Roles = roles;
            return PartialView("EditUserPartial", model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var user = DbContext.AspNetUsers.Include(u => u.AspNetUserRoles).Single(u => u.Id == model.Id);

            user = model.ToDbModel(ref user);

            if (!string.IsNullOrWhiteSpace(model.RoleId))
            {
                if (!user.AspNetUserRoles.Any(r => r.RoleId == model.RoleId))
                {
                    var role = DbContext.AspNetRoles.Single(r => r.Id == model.RoleId);

                    user.AspNetUserRoles.Clear();
                    user.AspNetUserRoles.Add(new AspNetUserRoles()
                    {
                        RoleId = role.Id,
                        UserId = user.Id
                    });
                }
            }
            else
            {
                if (user.AspNetUserRoles.Count > 0)
                    user.AspNetUserRoles.Clear();
            }


            DbContext.Entry(user).State = EntityState.Modified;
            DbContext.SaveChanges();

            var result = new IdentityResult();
            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                var appUser = await _userManager.FindByIdAsync(model.Id);

                await _userManager.RemovePasswordAsync(appUser);
                result = await _userManager.AddPasswordAsync(appUser, model.Password);
            }

            return Json(new
            {
                result = result.Succeeded,
                errors = result.Errors
            });
        }

        public IActionResult Logs(LogsReportViewModel model)
        {
            var pagedList = _logsReportService
                .GetLogs(model.TopId ?? -1, model.Page ?? 1, ITEMS_PER_PAGE, model.LogLevel, DbContext);

            model.LogItems = new StaticPagedList<LogEntryViewModel>(pagedList.Select(LogEntryViewModel.FromDbModel),
                pagedList.GetMetaData());

            IActionResult result;
            if (!model.TopId.HasValue)
            {
                model.TopId = model.LogItems.FirstOrDefault()?.Log.Id;
            }
            if (IsAjaxRequest())
            {
                result = PartialView("LogsList", model.LogItems);
            }
            else
            {
                ViewBag.LevelFilterOptions = new SelectList(_logsReportService.GetLogLevelOptions(), "Value", "Text");
                result = View(model);
            }

            return result;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Users");
        }
    }
}