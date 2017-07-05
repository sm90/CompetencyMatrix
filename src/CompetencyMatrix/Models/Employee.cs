using CompetencyMatrix.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompetencyMatrix.Models
{
    public partial class Employee
    {
        public Employee()
        {
            EmployeePastProject = new HashSet<EmployeePastProject>();
        }

        public int Id { get; set; }

        public int MatrixId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public byte[] Photo { get; set; }
        public EmplyeeProfileStatus ProfileStatus { get; set; }
        public string Email { get; set; }
        public int? Manager { get; set; }
        public string Skype { get; set; }
        public string Cell { get; set; }
        public string Office { get; set; }

        public EmployeeMatrixApproval MatrixApproval { get; set; }

        public virtual ICollection<EmployeePastProject> EmployeePastProject { get; set; }
        public virtual Employee ManagerNavigation { get; set; }
        public virtual ICollection<Employee> InverseManagerNavigation { get; set; }
        public virtual EmployeeMatrix Matrix { get; set; }
    }
}
