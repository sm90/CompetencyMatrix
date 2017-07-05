using System;
using System.Collections.Generic;

namespace CompetencyMatrix.Models
{
    public partial class SkillEvaluationModel
    {
        public SkillEvaluationModel()
        {
            Skill = new HashSet<Skill>();
            SkillEvaluationModelLevel = new HashSet<SkillEvaluationModelLevel>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Skill> Skill { get; set; }
        public virtual ICollection<SkillEvaluationModelLevel> SkillEvaluationModelLevel { get; set; }
    }
}
