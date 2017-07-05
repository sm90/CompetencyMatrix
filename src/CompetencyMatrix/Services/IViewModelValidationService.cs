using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CompetencyMatrix.ViewModels;

namespace CompetencyMatrix.Services
{
    public interface IViewModelValidationService
    {
        IEnumerable<ValidationResult> Validate(PositionMatrixInheritanceManagementViewModel viewModel);
    }
}