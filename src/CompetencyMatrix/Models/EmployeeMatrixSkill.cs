using System;
using System.Linq;
using System.Collections.Generic;


namespace CompetencyMatrix.Models
{
    public partial class EmployeeMatrixSkill
    {
        public int Id { get; set; }
        public int MatrixId { get; set; }
        public int SkillId { get; set; }
        public int SkillLevelId { get; set; }

        public virtual EmployeeMatrix Matrix { get; set; }
        public virtual Skill Skill { get; set; }
        public virtual SkillLevelModel SkillLevel { get; set; }

        public static List<EmployeeMatrixSkill> FromChangeLogDbModel(IList<ChangeLog> items, List<SkillLevelModel> levels)
        {
            var skills = new List<EmployeeMatrixSkill>();

            foreach (var changeLog in items)
            {
                skills.Add(new EmployeeMatrixSkill()
                {
                    MatrixId = changeLog.MatrixId,
                    Skill = changeLog.Skill,
                    SkillId = changeLog.SkillId,
                    SkillLevelId = changeLog.SkillLevelId,
                    SkillLevel = levels.First(x => x.Id == changeLog.SkillLevelId)
                });
            }

            return skills;
        }

        public static EmployeeMatrixSkill FromChangeLogDbModel(ChangeLog changeLog, List<SkillLevelModel> levels)
        {
            return new EmployeeMatrixSkill()
            {
                MatrixId = changeLog.MatrixId,
                Skill = changeLog.Skill,
                SkillId = changeLog.SkillId,
                SkillLevelId = changeLog.SkillLevelId,
                SkillLevel = levels.First(x => x.Id == changeLog.SkillLevelId)
            };
        }
    }
}
