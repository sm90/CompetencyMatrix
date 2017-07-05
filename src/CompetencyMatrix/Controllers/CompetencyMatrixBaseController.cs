using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using CompetencyMatrix.Infrastructure;
using CompetencyMatrix.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CompetencyMatrix.Controllers
{
    /// <summary>
    /// Represent the network logics for Employees window
    /// </summary>
    [Controller]
    [Authorize]
    public class CompetencyMatrixBaseController : AuthorizedControllerBase
    {
        public CompetencyMatrixBaseController ( CompetencyMatrixContext context, IServerVariables serverVariables ) : base ( context, serverVariables )
        {
        }

        public FileStreamResult GetEmployeePhoto(int id)
        {
            return GetFile(id);
        }

        protected FileStreamResult GetFile(int employeeId)
        {
            var employee = DbContext.Employee.SingleOrDefault(a => a.Id == employeeId) ?? default(Employee);

            Stream stream = new MemoryStream(employee.Photo);
            return new FileStreamResult(stream, "image/jpeg");
        }
        internal string GetUserId()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            if (claimsIdentity != null)
            {
                // the principal identity is a claims identity.
                // now we need to find the NameIdentifier claim
                var userIdClaim = claimsIdentity.Claims
                    .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

                if (userIdClaim != null)
                {
                    return userIdClaim.Value;
                }
            }
            return String.Empty;
        }

        protected void ClearSessionAfterUseEmployeeSkill()
        {
            ServerVariables.CurrentUserSkills = null;
        }

        protected JsonResult MakeJsonResult(Action action)
        {
            return MakeJsonResult(() =>
            {
                action();
                return null;
            });
        }

        public JsonResult MakeJsonResult(Func<string> action)
        {
            string message = String.Empty, stackTrace = null;
            bool successful = false;
            try
            {
                string result = action();

                message = string.IsNullOrWhiteSpace(result) ?
                    "Successful " :
                    result;
                successful = true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                if (ex.InnerException != null)
                    message += string.Format($"{Environment.NewLine}InnerException: {ex.InnerException.Message}");
            }

            return Json(new
            {
                successful,
                message,
                stackTrace,
                status = message
            });
        }

        protected int GetMatrixIdByUser(int userId)
        {
            return DbContext.Employee.Single(a => a.Id == userId).MatrixId;
        }

        public IActionResult Error() => View();

        public IActionResult NotFoundError() => View();

        public IActionResult InternalError() => View();

        public bool IsAjaxRequest()
        {
            return Request?.Headers != null && Request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }
    }
}
