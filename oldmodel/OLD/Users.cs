using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompetencyMatrix.Models
{
    public partial class Users
    {
        public Users()
        {
            ChangeLog = new HashSet<ChangeLog>();
            EmployeeMatrixApproval = new HashSet<EmployeeMatrixApproval>();
        }

        public int Id { get; set; }
        public int Name { get; set; }
        public int? EmployeeId { get; set; }

        [InverseProperty("ByWhomNavigation")]
        public virtual ICollection<ChangeLog> ChangeLog { get; set; }
        [InverseProperty("ByWhomNavigation")]
        public virtual ICollection<EmployeeMatrixApproval> EmployeeMatrixApproval { get; set; }
        [ForeignKey("EmployeeId")]
        [InverseProperty("Users")]
        public virtual Employee Employee { get; set; }
    }
}
