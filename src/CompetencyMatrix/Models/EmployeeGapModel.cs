using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompetencyMatrix.Models
{
    public class EmployeeGapModel
    {
        public string SkillName
        {
            get
            {
                if (PositionSkill != null)
                {
                    return PositionSkill.Skill.Name;
                }

                return EmployeeSkill.Skill.Name;
            }
        }

        public EmployeeMatrixSkill EmployeeSkill { get; set; }

        public PositionMatrixSkill PositionSkill { get; set; }

        public int Score
        {
            get
            {
                var score = 0;

                if (EmployeeSkill != null && PositionSkill != null)
                {
                    if (EmployeeSkill.SkillLevel.Quality == PositionSkill.SkillLevel.Quality)
                    {
                        score = 100;
                    }
                    if(EmployeeSkill.SkillLevel.Quality > PositionSkill.SkillLevel.Quality)
                    {
                        score = 100 + (EmployeeSkill.SkillLevel.Quality - PositionSkill.SkillLevel.Quality);
                    }
                }

                return score;
            }
        }

    }
}
