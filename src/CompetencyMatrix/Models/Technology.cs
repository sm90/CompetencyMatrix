using System;
using System.Collections.Generic;

namespace CompetencyMatrix.Models
{
    public partial class Technology
    {
        public Technology()
        {
            EmployeePastProjectTechnology = new HashSet<EmployeePastProjectTechnology>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<EmployeePastProjectTechnology> EmployeePastProjectTechnology { get; set; }
    }
}
