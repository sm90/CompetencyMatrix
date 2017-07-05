using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CompetencyMatrix.Controllers;
using CompetencyMatrix.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace CompetencyMatrix.Infrastructure.Security
{
    public class PositionMatrixAuthorizationHandler :
        AuthorizationHandler<OperationAuthorizationRequirement, PositionMatrix>
    {
        private const string MODIFY_OWN_MATRIX = "ModifyOwnMatrix";
        private const string MODIFY_ANY_MATRIX = "ModifyAnyMatrix";

        protected override Task HandleRequirementAsync ( AuthorizationHandlerContext context,
            OperationAuthorizationRequirement requirement,
            PositionMatrix resource )
        {

            var user = context.User as UserWithPermissionsPrincipal;
            var userId = user.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (requirement == Operations.Delete)
            {
                if ( user.Permissions.Any ( p => p.Name == MODIFY_ANY_MATRIX ) 
                    || (!resource.IsPublic && resource.OwnerId == userId && user.Permissions.Any(p => p.Name == MODIFY_OWN_MATRIX)) )
                {
                    context.Succeed(requirement);
                }
            }
            else if (requirement == Operations.Update)
            {
                if ( user.Permissions.Any ( p => p.Name == MODIFY_ANY_MATRIX )
                    || ( resource.OwnerId == userId && user.Permissions.Any ( p => p.Name == MODIFY_OWN_MATRIX )))
                {
                    context.Succeed(requirement);
                }
            }

            return Task.CompletedTask;
        }
    }
}