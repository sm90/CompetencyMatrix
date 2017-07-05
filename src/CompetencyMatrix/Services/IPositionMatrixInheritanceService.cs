using System.Collections.Generic;
using CompetencyMatrix.Models;

namespace CompetencyMatrix.Services
{
    public interface IPositionMatrixInheritanceService
    {
        void PopulateDescendants(PositionMatrix matrix);

        void PopulateAncestors(PositionMatrix matrix);

        List<PositionMatrix> GetPossibleParents(PositionMatrix matrix);

        void RelinkChildrenToParents(PositionMatrix matrix, ICompetencyMatrixContext dbContext);

        void RelinkChildToParents(PositionMatrix inheritanceParentMatrix, PositionMatrix positionMatrix,
            ICompetencyMatrixContext dbContext);
    }
}