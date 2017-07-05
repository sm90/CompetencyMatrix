using System.Collections.Generic;
using CompetencyMatrix.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CompetencyMatrix.Services
{
    public class PositionMatrixInheritanceService : IPositionMatrixInheritanceService
    {
        private readonly ICompetencyMatrixContext _dbContext;

        public PositionMatrixInheritanceService(ICompetencyMatrixContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void PopulateDescendants(PositionMatrix matrix)
        {
            matrix.AllChildren.Clear();
            AddChildren(matrix, matrix.AllChildren);
        }

        public void PopulateAncestors(PositionMatrix matrix)
        {
            matrix.AllParents.Clear();
            AddParents(matrix, matrix.AllParents);
        }

        public List<PositionMatrix> GetPossibleParents(PositionMatrix matrix)
        {
            var matrixesCanBeAdded = _dbContext.PositionMatrix.Where(x =>
                !x.IsDeleted && x.Id != matrix.Id &&
                matrix.AllChildren.All(childMatrix => childMatrix.Id != x.Id) &&
                matrix.AllParents.All(parentMatrix => parentMatrix.Id != x.Id));

            return matrixesCanBeAdded.ToList();
        }

        public void RelinkChildrenToParents(PositionMatrix matrix, ICompetencyMatrixContext dbContext)
        {
            var currentParentIds = dbContext.PositionMatrixInheritance
                .Where(e => e.MatrixId == matrix.Id)
                .Select(x => x.ParentMatrixId)
                .ToList();

            var currentChildrenInheritances = dbContext.PositionMatrixInheritance
                .Where(e => e.ParentMatrixId == matrix.Id)
                .ToList();

            foreach (var childLink in currentChildrenInheritances)
            {
                var childMatrix = dbContext.PositionMatrix
                    .Include(m => m.PositionMatrixInheritanceParentMatrix)
                    .Single(x => x.Id == childLink.MatrixId);

                LinkMatrixToParentIds(childMatrix, currentParentIds);
            }
        }

        public void RelinkChildToParents(PositionMatrix matrix, PositionMatrix childMatrix,
            ICompetencyMatrixContext dbContext)
        {
            var parentIds = dbContext.PositionMatrixInheritance
                .Where(e => e.MatrixId == matrix.Id)
                .Select(x => x.ParentMatrixId)
                .ToList();

            LinkMatrixToParentIds(childMatrix, parentIds);
        }

        private void LinkMatrixToParentIds(PositionMatrix childMatrix, List<int> parentIds)
        {
            foreach (var parentId in parentIds)
            {
                if (childMatrix.PositionMatrixInheritanceMatrix.All(x => x.ParentMatrixId != parentId))
                {
                    childMatrix.PositionMatrixInheritanceMatrix.Add(new PositionMatrixInheritance
                    {
                        Matrix = childMatrix,
                        ParentMatrixId = parentId
                    });
                }
            }
        }

        private void AddParents(PositionMatrix matrix, List<PositionMatrix> parents)
        {
            var currentParentInheritances = matrix.PositionMatrixInheritanceMatrix.Where(e => e.MatrixId == matrix.Id);


            foreach (var currentParentInheritance in currentParentInheritances)
            {
                currentParentInheritance.ParentMatrix = _dbContext.PositionMatrix
                    .Include(x => x.PositionMatrixInheritanceMatrix)
                    .Include(x => x.Owner)
                    .Single(x => x.Id == currentParentInheritance.ParentMatrixId);

                parents.Add(currentParentInheritance.ParentMatrix);
                AddParents(currentParentInheritance.ParentMatrix, parents);
            }
        }

        private void AddChildren(PositionMatrix matrix, List<PositionMatrix> children)
        {
            var currentChildrenInheritances =
                matrix.PositionMatrixInheritanceParentMatrix.Where(e => e.ParentMatrixId == matrix.Id);

            foreach (var currentChildrenInheritance in currentChildrenInheritances)
            {
                currentChildrenInheritance.Matrix = _dbContext.PositionMatrix
                    .Include(x => x.PositionMatrixInheritanceParentMatrix)
                    .Include(x => x.Owner)
                    .Single(x => x.Id == currentChildrenInheritance.MatrixId);

                children.Add(currentChildrenInheritance.Matrix);
                AddChildren(currentChildrenInheritance.Matrix, children);
            }
        }
    }
}