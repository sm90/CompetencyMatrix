using System.Linq;
using System.Threading.Tasks;
using CompetencyMatrix.Infrastructure;
using CompetencyMatrix.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApplicationCore.Models.AccountViewModels;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CompetencyMatrix.Controllers
{
    [RequireHttps]
    [AllowAnonymous]
    public class AccountController : CompetencyMatrixBaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger _logger;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILoggerFactory loggerFactory,
            CompetencyMatrixContext context,
            IServerVariables serverVariables) : base(context, serverVariables)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = loggerFactory.CreateLogger<AccountController>();
        }

        //
        // GET: /Account/Login
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult LoginPopup()
        {
            return View();
        }


        public IActionResult ReLogin(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return RedirectToLocal(Url.Action("Login", "Account"));
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await LoginUser(model);
                if (result)
                {
                    if (string.IsNullOrEmpty(returnUrl))
                        return RedirectToAction("PositionMatrices", "Home");
                    return Redirect(returnUrl);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        private async Task<bool> LoginUser(LoginViewModel model)
        {
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, set lockoutOnFailure: true
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, lockoutOnFailure: false);
            if (true || result.Succeeded)
            {

                _logger.LogInformation(1, "User logged in.");
                //var user = await _userManager.FindByEmailAsync(model.Email);
                string userId = null;// = user.Id;

                //TODO remove before going life
                if (model.Email == "admin@intetics.com")
                {
                    userId = "24d105e3-9693-4c81-8f67-2758b518d2d1";
                }
                else
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    userId = user.Id;
                }

                ServerVariables.CurrentAspUserId = userId;
                var employee = DbContext.AspNetUsers.SingleOrDefault(a => string.CompareOrdinal(a.Id, userId) == 0);

                ServerVariables.CurrentUserId = employee.EmployeeId ?? -1;


                var positionMatrix = DbContext.PositionMatrix.SingleOrDefault(a => a.Id == ServerVariables.CurrentUserId);
                ServerVariables.CurrentUserPosition = positionMatrix == null ? "Unknown position" : positionMatrix.Name;
                return true;
            }
            else
            {
                return false;
            }
        }



        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<object> LoginPopup(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await LoginUser(model);
                if (result)
                {
                    return new
                    {
                        Success = true
                    };
                }
                else
                {
                    return new
                    {
                        Message = "Invalid login attempt.",
                        Success = false
                    };
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation(4, "User logged out.");
            return RedirectToAction(nameof(AccountController.Login), "Account");
        }

        public IActionResult Forbidden()
        {
            return View();
        }

        #region Helpers

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        #endregion
    }
}
