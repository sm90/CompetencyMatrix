using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompetencyMatrix.Models
{
    public partial class PositionMatrixSkillGroups
    {
        public PositionMatrixSkillGroups()
        {
            PositionMatrixSkills = new HashSet<PositionMatrixSkills>();
        }

        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int GroupTypeId { get; set; }

        [InverseProperty("SkillGroup")]
        public virtual ICollection<PositionMatrixSkills> PositionMatrixSkills { get; set; }
        [ForeignKey("GroupTypeId")]
        [InverseProperty("PositionMatrixSkillGroups")]
        public virtual SkillGroupTypes GroupType { get; set; }
    }
}
