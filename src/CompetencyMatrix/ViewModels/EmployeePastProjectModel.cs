using System;
using System.Collections.Generic;
using CompetencyMatrix.Models;

namespace CompetencyMatrix.ViewModels
{
    public class EmployeePastProjectModel
    {
        public int EmployeePastProjectId { get; set; }
        public string CompanyName { get; set; }
        public string WorkPeriod { get; set; }
        public string Project { get; set; }
        public string ProjectDescription { get; set; }
        public string Role { get; set; }
        public string Technologies { get; set; }
        public string Tools { get; set; }
        public string Team { get; set; }
        public string DescriptionTooltip { get; set; }
        public static IList<EmployeePastProjectModel> FromDbModel(List<EmployeePastProjectModel> items)
        {
            IList<EmployeePastProjectModel> model = new List<EmployeePastProjectModel>();
         
            return model;
        }
    }
}
