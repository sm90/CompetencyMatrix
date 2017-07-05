using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CompetencyMatrix.Services;

namespace CompetencyMatrix.ViewModels
{
    public class PositionMatrixInheritanceManagementViewModel : IValidatableObject
    {
        public PositionMatrixInheritanceManagementViewModel()
        {
            MatrixesCanBeAddedToParent = new PositionMatrixList();
            ParentMatrixes = new PositionMatrixList();
        }

        public PositionMatrixDetails CurrentMatrix { get; set; }

        public PositionMatrixList MatrixesCanBeAddedToParent { get; set; }

        public PositionMatrixList ParentMatrixes { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationService =
                (IViewModelValidationService) validationContext.GetService(typeof(IViewModelValidationService));

            return validationService.Validate(this);
        }
    }
}