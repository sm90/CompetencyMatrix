using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace CompetencyMatrix.Models
{
    public static class Operations
    {
        public static readonly OperationAuthorizationRequirement Create =
            new OperationAuthorizationRequirement {Name = "Create"};

        public static readonly OperationAuthorizationRequirement Read =
            new OperationAuthorizationRequirement {Name = "Read"};

        public static readonly OperationAuthorizationRequirement Update =
            new OperationAuthorizationRequirement {Name = "Update"};

        public static readonly OperationAuthorizationRequirement Delete =
            new OperationAuthorizationRequirement {Name = "Delete"};
    }
}