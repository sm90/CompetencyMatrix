using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CompetencyMatrix.ViewModels
{
    public class TrainingSertificationModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        
        public int TypeId { get; set; }
        
        public string Type { get; set; }

        public int EmployeeId { get; set; }

        [Required]
        [Display(Name = "Date")]
        public DateTime When { get; set; }

        public string WhenYear { get; set; }


        public static TrainingSertificationModel FromDbModel(Models.TrainingSertification model)
        {
            var viewModel = new TrainingSertificationModel();

            viewModel.Id = model.Id;
            viewModel.Name = model.Name;
            viewModel.TypeId = model.TypeId;
            viewModel.Type = model.Type?.Name;
            viewModel.When = model.When;
            viewModel.WhenYear = model.When.Year.ToString();
            viewModel.EmployeeId = model.EmployeeId;

            return viewModel;
        }

    }
}
