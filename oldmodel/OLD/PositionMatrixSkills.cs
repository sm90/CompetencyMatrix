using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompetencyMatrix.Models
{
    public partial class PositionMatrixSkills
    {
        [Column("id")]
        public int Id { get; set; }
        public int MatrixId { get; set; }
        public int SkillId { get; set; }
        public int SkillLevelId { get; set; }
        public int SkillGroupId { get; set; }

        [ForeignKey("MatrixId")]
        [InverseProperty("PositionMatrixSkills")]
        public virtual PositionMatrix Matrix { get; set; }
        [ForeignKey("SkillGroupId")]
        [InverseProperty("PositionMatrixSkills")]
        public virtual PositionMatrixSkillGroups SkillGroup { get; set; }
        [ForeignKey("SkillId")]
        [InverseProperty("PositionMatrixSkills")]
        public virtual Skills Skill { get; set; }
        [ForeignKey("SkillLevelId")]
        [InverseProperty("PositionMatrixSkills")]
        public virtual AvailableSkillLevels SkillLevel { get; set; }
    }
}
