using CompetencyMatrix.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompetencyMatrix.ViewModels
{
    public class PositionMatrixSkillListItem
    {
        public int Id { get; set; }
        public virtual int MatrixId { get; set; }
        public virtual string MatrixName { get; set; }
        public virtual string SkillLevelName { get; set; }
        public virtual int SkillLevelId { get; set; }
        public virtual string SkillName { get; set; }
        public virtual int SkillId { get; set; }


        public virtual int? SkillGroupId { get; set; }

        public int? SkillGroupTypeId { get; set; }

        public virtual int? EvaluationModelId { get; set; }

        public bool Hidden { get; set; }

        public bool Leaf { get; set; }

        public EntityState State { get; set; } = EntityState.Unchanged;

        public bool IsInherited { get; set; }

        public static PositionMatrixSkillListItem FromDbModel(Skill skill)
        {
            var result = new PositionMatrixSkillListItem();

            result.Id = skill.Id;
            result.SkillId = skill.Id;
            result.SkillName = skill.Name;
            result.SkillLevelId = skill.EvaluationModel.SkillEvaluationModelLevel.First().SkillLevelModelId;
            result.EvaluationModelId = skill.EvaluationModelId;

            return result;
        }

        public static PositionMatrixSkillListItem FromDbModel(PositionMatrixSkill skill)
        {
            var result = new PositionMatrixSkillListItem();

            result.Id = skill.Id;
            result.SkillId = skill.Skill.Id;
            result.SkillName = skill.Skill.Name;

            result.SkillLevelId = skill.SkillLevelId;
            result.SkillLevelName = skill.SkillLevel.Name;

            result.SkillGroupId = skill.SkillGroupId;
            result.SkillGroupTypeId = skill.SkillGroupTypeId;

            result.MatrixId = skill.MatrixId;
            result.MatrixName = skill.Matrix.Name;

            result.EvaluationModelId = skill.Skill.EvaluationModelId;

            result.Hidden = skill.Hidden;

            result.IsInherited = skill.IsInherited;

            if (skill.SkillGroupId == null)
            {
                result.Leaf = true;
            }

            return result;
        }


        public static PositionMatrixSkill ToDbModel(PositionMatrixSkillListItem skill)
        {
            var result = new PositionMatrixSkill();

            result.SkillGroupId = skill.SkillGroupId;

            if (skill.State == EntityState.Added)
            {
                result.Id = 0;
            }
            else
            {
                result.Id = skill.Id;

                if (skill.SkillGroupId <= 0)
                {
                    //it is skill from new group
                    result.SkillGroupId = 0;
                }
            }

            result.SkillId = skill.SkillId;
            result.MatrixId = skill.MatrixId;
            result.SkillLevelId = skill.SkillLevelId;
            result.SkillGroupTypeId = skill.SkillGroupTypeId;
            result.Hidden = skill.Hidden;

            return result;
        }


    }

}