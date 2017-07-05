using System;
using System.Collections.Generic;

namespace CompetencyMatrix.Models
{
    public partial class SkillLevelModel
    {
        public SkillLevelModel()
        {
            EmployeeMatrixSkill = new HashSet<EmployeeMatrixSkill>();
            PositionMatrixSkill = new HashSet<PositionMatrixSkill>();
            SkillEvaluationModelLevel = new HashSet<SkillEvaluationModelLevel>();
            SkillLevelCriteria = new HashSet<SkillLevelCriteria>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quality { get; set; }

        public virtual ICollection<EmployeeMatrixSkill> EmployeeMatrixSkill { get; set; }
        public virtual ICollection<PositionMatrixSkill> PositionMatrixSkill { get; set; }
        public virtual ICollection<SkillEvaluationModelLevel> SkillEvaluationModelLevel { get; set; }
        public virtual ICollection<SkillLevelCriteria> SkillLevelCriteria { get; set; }
    }
}
