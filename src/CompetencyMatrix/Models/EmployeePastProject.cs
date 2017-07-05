using System;
using System.Collections.Generic;

namespace CompetencyMatrix.Models
{
    public partial class EmployeePastProject
    {
        public int Id { get; set; }
        public string Company { get; set; }
        public DateTime WorkPeriodStart { get; set; }
        public DateTime? WorkPeriodEnd { get; set; }
        public string Description { get; set; }
        public string Role { get; set; }
        public string Team { get; set; }
        public string Project { get; set; }
        public int EmployeeId { get; set; }
        public string Tool { get; set; }
        public string Technology { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
