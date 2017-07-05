using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompetencyMatrix.Models
{
    public partial class PositionMatrix
    {
        public PositionMatrix()
        {
            AllChildren = new List<PositionMatrix>();
            AllParents = new List<PositionMatrix>();
            PositionMatrixInheritanceMatrix = new HashSet<PositionMatrixInheritance>();
            PositionMatrixInheritanceParentMatrix = new HashSet<PositionMatrixInheritance>();
            PositionMatrixSkill = new HashSet<PositionMatrixSkill>();
            PositionMatrixSkillGroups = new List<PositionMatrixSkillGroup>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsPublic { get; set; }

        public string OwnerId { get; set; }

        public AspNetUsers Owner { get; set; }

        public virtual ICollection<PositionMatrixInheritance> PositionMatrixInheritanceMatrix { get; set; }
        public virtual ICollection<PositionMatrixInheritance> PositionMatrixInheritanceParentMatrix { get; set; }
        public virtual ICollection<PositionMatrixSkill> PositionMatrixSkill { get; set; }

        [NotMapped]
        public virtual ICollection<PositionMatrixSkillGroup> PositionMatrixSkillGroups { get; set; }

        [NotMapped]
        public List<PositionMatrix> AllParents { get; }

        [NotMapped]
        public List<PositionMatrix> AllChildren { get; }
    }
}
