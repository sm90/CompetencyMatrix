using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompetencyMatrix.Models
{
    public partial class SkillLevels
    {
        [Column("id")]
        public int Id { get; set; }
        public int SkillId { get; set; }
        public int SkillLevelId { get; set; }

        [ForeignKey("SkillId")]
        [InverseProperty("SkillLevels")]
        public virtual Skills Skill { get; set; }
        [ForeignKey("SkillLevelId")]
        [InverseProperty("SkillLevels")]
        public virtual AvailableSkillLevels AvailableSkillLevel { get; set; }
    }
}
