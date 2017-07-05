using CompetencyMatrix.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompetencyMatrix.Models
{
    public class TrainingSertification
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int TypeId { get; set; }

        public TrainingSertificationType Type { get; set; }

        public DateTime When { get; set; }

        public int EmployeeId { get; set; }

        public Employee Employee { get; set; }

        public static TrainingSertification FromModel(TrainingSertificationModel model)
        {
            var viewModel = new TrainingSertification();

            viewModel.Id = model.Id;
            viewModel.Name = model.Name;
            viewModel.TypeId = model.TypeId;
            viewModel.EmployeeId = model.EmployeeId;
            viewModel.When = model.When;

            return viewModel;
        }
    }


}
