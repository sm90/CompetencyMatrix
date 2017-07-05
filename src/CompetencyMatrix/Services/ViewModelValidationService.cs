using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CompetencyMatrix.Models;
using CompetencyMatrix.ViewModels;

namespace CompetencyMatrix.Services
{
    public class ViewModelValidationService : IViewModelValidationService
    {
        private readonly ICompetencyMatrixContext _dbContext;
        private readonly IPositionMatrixService _positionMatrixService;
        private readonly IPositionMatrixInheritanceService _positionMatrixInheritanceService;

        public ViewModelValidationService(ICompetencyMatrixContext dbContext,
            IPositionMatrixService positionMatrixService,
            IPositionMatrixInheritanceService positionMatrixInheritanceService)
        {
            _dbContext = dbContext;
            _positionMatrixService = positionMatrixService;
            _positionMatrixInheritanceService = positionMatrixInheritanceService;
        }

        public IEnumerable<ValidationResult> Validate(PositionMatrixInheritanceManagementViewModel viewModel)
        {
            var currentMatrix = _positionMatrixService.GetFullPositionMatrixbyId(viewModel.CurrentMatrix.Id);

            if (currentMatrix == null)
            {
                yield return
                    new ValidationResult($"Current matrix {viewModel.CurrentMatrix.Name} is not found in DataBase");
            }
            else
            {
                var matrixesCanBeAdded = _positionMatrixInheritanceService.GetPossibleParents(currentMatrix);

                var validParentIds = matrixesCanBeAdded
                    .Select(m => m.Id)
                    .Union(currentMatrix.PositionMatrixInheritanceMatrix.Select(m => m.ParentMatrixId))
                    .Distinct().ToList();

                foreach (var item in viewModel.ParentMatrixes)
                {
                    var parentMatrix = _dbContext.PositionMatrix
                        .Where(x => !x.IsDeleted)
                        .SingleOrDefault(x => x.Id == item.Id);

                    if (parentMatrix == null)
                    {
                        yield return
                            new ValidationResult($"Matrix {item.Name} is not found in DataBase");
                    }
                    else
                    {
                        if (!validParentIds.Contains(item.Id))
                        {
                            yield return
                                new ValidationResult($"Matrix {item.Name} cannot be added due to Inheritance Loop");
                        }
                    }
                }
            }
        }
    }
}