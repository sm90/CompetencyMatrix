using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompetencyMatrix.Models
{
    public partial class SkillCriteria
    {
        public SkillCriteria()
        {
            SkillLevelCriteria = new HashSet<SkillLevelCriteria>();
        }

        public int Id { get; set; }
        public int SkillId { get; set; }
        [Required]
        public string Name { get; set; }
        [Column(TypeName = "ntext")]
        public string Description { get; set; }

        [InverseProperty("SkillCriterion")]
        public virtual ICollection<SkillLevelCriteria> SkillLevelCriteria { get; set; }
        [ForeignKey("SkillId")]
        [InverseProperty("SkillCriteria")]
        public virtual Skills Skill { get; set; }
    }
}
