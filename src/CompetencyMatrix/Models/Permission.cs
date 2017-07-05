using System;
using System.Collections.Generic;

namespace CompetencyMatrix.Models
{
    public partial class Permission
    {
        public Permission()
        {
            PermissionOnRole = new HashSet<PermissionOnRole>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }

        public virtual ICollection<PermissionOnRole> PermissionOnRole { get; set; }
    }
}
