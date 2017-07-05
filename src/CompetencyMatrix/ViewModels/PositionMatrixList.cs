using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompetencyMatrix.Models;

namespace CompetencyMatrix.ViewModels
{
    public class PositionMatrixList: List<PositionMatrixListItem>
    {
        public static PositionMatrixList FromDbModel(IEnumerable<PositionMatrix> matrices)
        {
            var result = new PositionMatrixList();
            result.AddRange(matrices.Select(PositionMatrixListItem.FromDbModel));

            return result;
        }
    }
}
