using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompetencyMatrix.Models
{
    public partial class SkillLevelCriteria
    {
        [Column("id")]
        public int Id { get; set; }
        public int SkillLevelId { get; set; }
        public int SkillCriterionId { get; set; }

        [ForeignKey("SkillCriterionId")]
        [InverseProperty("SkillLevelCriteria")]
        public virtual SkillCriteria SkillCriterion { get; set; }
        [ForeignKey("SkillLevelId")]
        [InverseProperty("SkillLevelCriteria")]
        public virtual AvailableSkillLevels AvailableSkillLevel { get; set; }
    }
}
