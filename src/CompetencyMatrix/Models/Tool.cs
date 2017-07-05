using System;
using System.Collections.Generic;

namespace CompetencyMatrix.Models
{
    public partial class Tool
    {
        public Tool()
        {
            EmployeePastProjectTool = new HashSet<EmployeePastProjectTool>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<EmployeePastProjectTool> EmployeePastProjectTool { get; set; }
    }
}
