using CompetencyMatrix.Models;
using CompetencyMatrix.ViewModels;

namespace CompetencyMatrix.Services
{
    public interface IPositionMatrixService
    {
        PositionMatrix GetPositionMatrix(int positionMatrixId);

        PositionMatrix GetFullPositionMatrixbyId(int positionMatrixId);

        void SetParentMatrixes(PositionMatrixInheritanceManagementViewModel viewModel);

        PositionMatrix Create(PositionMatrixDetails viewModel);

        void DeleteMatrix(int positionMatrixId);

        PositionMatrixSkills GetPositionMatrixSkills(int positionMatrixId);

        ViewModels.PositionMatrixSkillGroup GetMatrixSkillData(int id, int? parentId, int matrixId, SkillViewType type);

        bool Update(PositionMatrixSkills skills);

        SkillGroupType GetDefaultSkillGroupType();
    }
}