using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompetencyMatrix.ViewModels
{
    public class TrainingSertificationTypeModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public static TrainingSertificationTypeModel FromDbModel(Models.TrainingSertificationType model)
        {
            var viewModel = new TrainingSertificationTypeModel();

            viewModel.Id = model.Id;
            viewModel.Name = model.Name;

            return viewModel;
        }
    }
}
