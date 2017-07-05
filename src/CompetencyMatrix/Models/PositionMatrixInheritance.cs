using System;
using System.Collections.Generic;

namespace CompetencyMatrix.Models
{
    public partial class PositionMatrixInheritance
    {
        public int Id { get; set; }
        public int MatrixId { get; set; }
        public int ParentMatrixId { get; set; }

        public virtual PositionMatrix Matrix { get; set; }
        public virtual PositionMatrix ParentMatrix { get; set; }
    }
}
