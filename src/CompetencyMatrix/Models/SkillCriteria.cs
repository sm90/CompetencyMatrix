using System;
using System.Collections.Generic;

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
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<SkillLevelCriteria> SkillLevelCriteria { get; set; }
        public virtual Skill Skill { get; set; }
    }
}
