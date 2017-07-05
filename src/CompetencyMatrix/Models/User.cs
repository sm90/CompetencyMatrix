using System;
using System.Collections.Generic;

namespace CompetencyMatrix.Models
{
    public partial class User
    {
        public User()
        {
            ChangeLog = new HashSet<ChangeLog>();
            EmployeeMatrixApproval = new HashSet<EmployeeMatrixApproval>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? EmployeeId { get; set; }
        public int RoleId { get; set; }

        public virtual ICollection<ChangeLog> ChangeLog { get; set; }
        public virtual ICollection<EmployeeMatrixApproval> EmployeeMatrixApproval { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Role Role { get; set; }
    }
}
