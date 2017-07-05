using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompetencyMatrix.Models
{
    public partial class EmployeeMatrix
    {
        public EmployeeMatrix()
        {
            ChangeLog = new HashSet<ChangeLog>();
            Employee = new HashSet<Employee>();
            EmployeeMatrixApproval = new HashSet<EmployeeMatrixApproval>();
            EmployeeMatrixSkills = new HashSet<EmployeeMatrixSkills>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        [Column(TypeName = "ntext")]
        public string Description { get; set; }

        [InverseProperty("Matrix")]
        public virtual ICollection<ChangeLog> ChangeLog { get; set; }
        [InverseProperty("Matrix")]
        public virtual ICollection<Employee> Employee { get; set; }
        [InverseProperty("Matrix")]
        public virtual ICollection<EmployeeMatrixApproval> EmployeeMatrixApproval { get; set; }
        [InverseProperty("Matrix")]
        public virtual ICollection<EmployeeMatrixSkills> EmployeeMatrixSkills { get; set; }
    }
}
