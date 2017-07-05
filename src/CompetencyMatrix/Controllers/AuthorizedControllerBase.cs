using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompetencyMatrix.Infrastructure;
using CompetencyMatrix.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApplicationCore.Models.AccountViewModels;
using System.Security.Claims;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CompetencyMatrix.Controllers
{
    [RequireHttps]
    [AllowAnonymous]
    public abstract class AuthorizedControllerBase : Controller
    {
        private const string ROLE_SCHEMA = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger _logger;
        private readonly ICompetencyMatrixContext _dbContext;

        private UserWithPermissionsPrincipal _userWithPermissions;

        public ICompetencyMatrixContext DbContext => _dbContext;
        public IServerVariables ServerVariables;
        public List<Permission> LoggedUserPermissions { get; set; }
        public UserWithPermissionsPrincipal UserWithPermissions
        {
            get
            {
                if ( _userWithPermissions == null)
                {
                    this.InitPermissions ();
                }
                return _userWithPermissions;
            }
            private set
            {
                _userWithPermissions = value;
            }
        }

        public AuthorizedControllerBase ( ) : base ( )
        {
        }

        public AuthorizedControllerBase (
            ICompetencyMatrixContext context,
            IServerVariables serverVariables )
        {
            _dbContext = context;
            ServerVariables = serverVariables;
        }

        public AuthorizedControllerBase (
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILoggerFactory loggerFactory,
            CompetencyMatrixContext context,
            IServerVariables serverVariables)
        {
            _dbContext = context;
            ServerVariables = serverVariables;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = loggerFactory.CreateLogger<AccountController>();
        }

        private void InitPermissions ( )
        {
            var userRole = this.User.Claims.FirstOrDefault ( x => x.Type == ROLE_SCHEMA);
            if ( userRole != null )
            {
                //var roles = this.DbContext.AspNetRoles.ToList ( );
                var permissions = (from role in this.DbContext.AspNetRoles
                                  join por in this.DbContext.PermissionOnRole
                                  on role.Id equals por.RoleId
                                  join permission in this.DbContext.Permission
                                  on por.PermissionId equals permission.Id
                                  where role.NormalizedName == userRole.Value.ToUpper()
                                  select permission).ToList();

                if ( permissions.Any())
                {
                    _userWithPermissions = new UserWithPermissionsPrincipal { User = this.User, Permissions = permissions };
                }
            }
        }
    }

    public class UserWithPermissionsPrincipal : ClaimsPrincipal
    {
        public ClaimsPrincipal User { get; set; }
        public List<Permission> Permissions { get; set; }
    }
}
