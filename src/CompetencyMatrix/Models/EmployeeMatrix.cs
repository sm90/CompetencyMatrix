using System;
using System.Collections.Generic;

namespace CompetencyMatrix.Models
{
    public partial class EmployeeMatrix
    {
        public EmployeeMatrix()
        {
            ChangeLog = new HashSet<ChangeLog>();
            Employee = new HashSet<Employee>();
            EmployeeMatrixSkill = new HashSet<EmployeeMatrixSkill>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<ChangeLog> ChangeLog { get; set; }
        public virtual ICollection<Employee> Employee { get; set; }
        public virtual ICollection<EmployeeMatrixSkill> EmployeeMatrixSkill { get; set; }
    }
}
