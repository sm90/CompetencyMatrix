using System;
using System.Collections.Generic;

namespace CompetencyMatrix.Models
{
    public partial class EmployeePastProjectTool
    {
        public int Id { get; set; }
        public int ToolId { get; set; }
        public int EmployeePastProjectId { get; set; }

        public virtual EmployeePastProject EmployeePastProject { get; set; }
        public virtual Tool Tool { get; set; }
    }
}
