using System.Threading.Tasks;
using CompetencyMatrix.Models;
using Microsoft.AspNetCore.Authorization;

namespace CompetencyMatrix.Infrastructure
{
    public class SkillLevelRequirement : IAuthorizationRequirement
    {
        public SkillLevelRequirement(int status)
        {
            MinimumStatus = status;
        }

        protected int MinimumStatus { get; set; }
    }

    public class SkillAccessHandler : AuthorizationHandler<SkillLevelRequirement>
    {
        private IServerVariables _variables;
        private CompetencyMatrixContext _context;



        public SkillAccessHandler(CompetencyMatrixContext context, IServerVariables variables)
        {
            _variables = variables;
            _context = context;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, SkillLevelRequirement requirement)
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
