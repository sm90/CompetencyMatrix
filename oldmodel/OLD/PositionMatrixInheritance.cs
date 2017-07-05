using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompetencyMatrix.Models
{
    public partial class PositionMatrixInheritance
    {
        [Column("id")]
        public int Id { get; set; }
        public int MatrixId { get; set; }
        public int ParentMatrixId { get; set; }

        [ForeignKey("MatrixId")]
        [InverseProperty("PositionMatrixInheritanceMatrix")]
        public virtual PositionMatrix Matrix { get; set; }
        [ForeignKey("ParentMatrixId")]
        [InverseProperty("PositionMatrixInheritanceParentMatrix")]
        public virtual PositionMatrix ParentMatrix { get; set; }
    }
}
