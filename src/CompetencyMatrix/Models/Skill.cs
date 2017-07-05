using System;
using System.Collections.Generic;

namespace CompetencyMatrix.Models
{
    public partial class Skill
    {
        public Skill()
        {
            ChangeLog = new HashSet<ChangeLog>();
            EmployeeMatrixSkill = new HashSet<EmployeeMatrixSkill>();
            PositionMatrixSkill = new HashSet<PositionMatrixSkill>();
            SkillCriteria = new HashSet<SkillCriteria>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public string TrainingMaterials { get; set; }
        public string Questionarie { get; set; }
        public int? EvaluationModelId { get; set; }

        public virtual ICollection<ChangeLog> ChangeLog { get; set; }
        public virtual ICollection<EmployeeMatrixSkill> EmployeeMatrixSkill { get; set; }
        public virtual ICollection<PositionMatrixSkill> PositionMatrixSkill { get; set; }
        public virtual ICollection<SkillCriteria> SkillCriteria { get; set; }
        public virtual SkillCategory Category { get; set; }
        public virtual SkillEvaluationModel EvaluationModel { get; set; }
    }
}
