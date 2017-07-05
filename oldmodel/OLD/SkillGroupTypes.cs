using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompetencyMatrix.Models
{
    public partial class SkillGroupTypes
    {
        public SkillGroupTypes()
        {
            PositionMatrixSkillGroups = new HashSet<PositionMatrixSkillGroups>();
        }

        [Column("id")]
        public int Id { get; set; }
        [Column("type")]
        public int Type { get; set; }

        [InverseProperty("GroupType")]
        public virtual ICollection<PositionMatrixSkillGroups> PositionMatrixSkillGroups { get; set; }
    }
}
