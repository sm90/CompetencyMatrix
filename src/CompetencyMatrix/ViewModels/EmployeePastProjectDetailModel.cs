using System;
using System.ComponentModel.DataAnnotations;

namespace CompetencyMatrix.ViewModels
{
    public class EmployeePastProjectDetailModel
    {
        public int EmployeeId { get; set; }

        public int Id { get; set; }
        [Display(Name = "Company")]
        public string Company { get; set; }

        [Display(Name = "Work Period From")]
        [DataType(DataType.Date)]
        [Required]
        public DateTime WorkPeriodStart { get; set; }

        [Display(Name = "Work Period To")]
        [DataType(DataType.Date)]
        public DateTime? WorkPeriodEnd { get; set; }

        [Display(Name = "Project")]
        [Required]
        public string Project { get; set; }

        [Display(Name = "Project description")]
        public string ProjectDescription { get; set; }

        [Display(Name = "Role in project")]
        [Required]
        public string Role { get; set; }

        [Display(Name = "Technologies")]
        public string Technologies { get; set; }

        [Display(Name = "Tools")]
        public string Tools { get; set; }

        [Display(Name = "Team")]
        public string Team { get; set; }

        public string WorkPeriodStartIso { get; set; }
        public string WorkPeriodEndIso { get; set; }


        public static EmployeePastProjectDetailModel CreateDefault()
        {
            EmployeePastProjectDetailModel model = new EmployeePastProjectDetailModel();
            model.WorkPeriodEnd = model.WorkPeriodStart = DateTime.Now;
            return model;
        }
    }
}
