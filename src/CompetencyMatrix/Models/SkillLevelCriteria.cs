using System;
using System.Collections.Generic;

namespace CompetencyMatrix.Models
{
    public partial class SkillLevelCriteria
    {
        public int Id { get; set; }
        public int SkillLevelModelId { get; set; }
        public int SkillCriteriaId { get; set; }

        public virtual SkillCriteria SkillCriteria { get; set; }
        public virtual SkillLevelModel SkillLevelModel { get; set; }
    }
}
