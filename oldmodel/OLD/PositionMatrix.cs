using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompetencyMatrix.Models
{
    public partial class PositionMatrix
    {
        public PositionMatrix()
        {
            PositionMatrixInheritanceMatrix = new HashSet<PositionMatrixInheritance>();
            PositionMatrixInheritanceParentMatrix = new HashSet<PositionMatrixInheritance>();
            PositionMatrixSkills = new HashSet<PositionMatrixSkills>();
        }

        [Column("id")]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Column(TypeName = "ntext")]
        public string Description { get; set; }

        [InverseProperty("Matrix")]
        public virtual ICollection<PositionMatrixInheritance> PositionMatrixInheritanceMatrix { get; set; }
        [InverseProperty("ParentMatrix")]
        public virtual ICollection<PositionMatrixInheritance> PositionMatrixInheritanceParentMatrix { get; set; }
        [InverseProperty("Matrix")]
        public virtual ICollection<PositionMatrixSkills> PositionMatrixSkills { get; set; }
    }
}
