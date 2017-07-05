using System;
using System.Collections.Generic;

namespace CompetencyMatrix.Models
{
    public partial class SkillEvaluationModelLevel
    {
        public int Id { get; set; }
        public int SkillEvaluationModelId { get; set; }
        public int SkillLevelModelId { get; set; }

        public virtual SkillEvaluationModel SkillEvaluationModel { get; set; }
        public virtual SkillLevelModel SkillLevelModel { get; set; }
    }
}
