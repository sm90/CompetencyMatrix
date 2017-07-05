using System;
using System.Collections.Generic;

namespace CompetencyMatrix.Models
{
    public partial class PermissionOnRole
    {
        public int Id { get; set; }
        public string RoleId { get; set; }
        public int PermissionId { get; set; }
        public bool IsActive { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }

        public virtual Permission Permission { get; set; }
        public virtual AspNetRoles Role { get; set; }
    }
}
