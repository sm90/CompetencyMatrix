using System;
using System.Collections.Generic;

namespace CompetencyMatrix.Models
{
    public partial class EmployeePastProjectTechnology
    {
        public int Id { get; set; }
        public int TechnologyId { get; set; }
        public int EmployeePastProjectId { get; set; }

        public virtual EmployeePastProject EmployeePastProject { get; set; }
        public virtual Technology Technology { get; set; }
    }
}
