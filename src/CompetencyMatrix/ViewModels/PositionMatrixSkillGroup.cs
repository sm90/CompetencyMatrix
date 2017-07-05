using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompetencyMatrix.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace CompetencyMatrix.ViewModels
{
    public class PositionMatrixSkillGroup
    {
        public PositionMatrixSkillGroup()
        {
        }

        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public virtual SkillGroupTypeViewModel GroupType { get; set; }

        private SkillGroupTypeQuality Quality { get; set; }

        public int GroupTypeId { get; set; }

        public virtual List<ViewModels.PositionMatrixSkillListItem> Skills { get; set; } = new List<ViewModels.PositionMatrixSkillListItem>();

        public SkillViewType SkillViewType { get; set; } = SkillViewType.Group;

        public List<PositionMatrixSkillGroup> ChildGroups { get; set; } = new List<ViewModels.PositionMatrixSkillGroup>();

        public int? ParentGroupId { get; set; }

        public int MatrixId { get; set; }

        public string MatrixName { get; set; }

        public bool Hidden { get; set; }

        public EntityState State { get; set; } = EntityState.Unchanged;

        public int? OverridenGroupId { get; set; }


        public static PositionMatrixSkillGroup FromDbModel(IGrouping<Models.PositionMatrixSkillGroup, Models.PositionMatrixSkill> group)
        {
            var result = new PositionMatrixSkillGroup();

            result.Id = group.Key.Id;
            result.Name = group.Key.Name;
            result.GroupTypeId = group.Key.GroupType.Id;
            result.GroupType = SkillGroupTypeViewModel.FromDbModel(group.Key.GroupType);
            result.ParentGroupId = group.Key.ParentGroupId;
            result.MatrixId = group.Key.MatrixId;
            result.OverridenGroupId = group.Key.OverridenGroupId;

            if (!result.OverridenGroupId.HasValue)
            {
                result.MatrixName = group.Key.Matrix.Name;
            }            
            
            result.Hidden = group.Key.Hidden;

            result.Skills = new List<PositionMatrixSkillListItem>();
            foreach (var dbSkill in group)
            {
                var listItem = PositionMatrixSkillListItem.FromDbModel(dbSkill);

                result.Skills.Add(listItem);
            }

            return result;
        }

        public static Models.PositionMatrixSkillGroup ToDbModel(PositionMatrixSkillGroup model, int matrixId, ICompetencyMatrixContext _dbContext)
        {
            var result = new Models.PositionMatrixSkillGroup();

            result.Name = model.Name;

            result.GroupTypeId = model.GroupTypeId;
            result.ParentGroupId = model.ParentGroupId;

            result.MatrixId = model.MatrixId;
            result.OverridenGroupId = model.OverridenGroupId;
            result.Hidden = model.Hidden;

            result.PositionMatrixSkill = new List<PositionMatrixSkill>();

            foreach (var skill in model.Skills)
            {
                var dbModel = PositionMatrixSkillListItem.ToDbModel(skill);

                if (model.State == EntityState.Added)
                {
                    dbModel.SkillGroupId = 0;
                }

                result.PositionMatrixSkill.Add(dbModel);
            }

            if (model.State == EntityState.Added)
            {
                result.Id = 0;

                if (result.ParentGroupId.HasValue && result.ParentGroupId.Value < 0)
                {
                    model.ParentGroupId = 0;
                }
            }
            else
            {
                result.Id = model.Id;
            }

            foreach (var group in model.ChildGroups)
            {

                var dbModel = PositionMatrixSkillGroup.ToDbModel(group, matrixId, _dbContext);

                if (group.MatrixId == matrixId && group.State != EntityState.Added)
                {
                    dbModel.ParentGroupId = 0;
                    _dbContext.Entry(dbModel).State = EntityState.Modified;
                }

                result.Children.Add(dbModel);
            }

            return result;
        }


        public static PositionMatrixSkillGroup FromDbModel(Models.PositionMatrixSkillGroup model, bool isEmptyGroup = false)
        {
            var result = new PositionMatrixSkillGroup();

            result.Id = model.Id;
            result.Name = model.Name;
            result.GroupTypeId = model.GroupTypeId;
            result.GroupType = SkillGroupTypeViewModel.FromDbModel(model.GroupType);
            result.MatrixId = model.MatrixId;
            result.OverridenGroupId = model.OverridenGroupId;

            if (!result.OverridenGroupId.HasValue)
            {
                result.MatrixName = model.Matrix.Name;
            }

            result.ParentGroupId = model.ParentGroupId;

            result.Hidden = model.Hidden;

            result.Skills = new List<PositionMatrixSkillListItem>();
            if (!isEmptyGroup)
            {
                foreach (var dbSkill in model.PositionMatrixSkill)
                {
                    var listItem = new PositionMatrixSkillListItem()
                    {
                        Id = dbSkill.Id,
                        SkillLevelName = dbSkill.SkillLevel.Name,
                        MatrixName = dbSkill.Matrix.Name,
                        SkillName = dbSkill.Skill.Name,
                        EvaluationModelId = dbSkill.Skill.EvaluationModelId
                    };

                    result.Skills.Add(listItem);
                }
            }

            return result;
        }

        public bool IsInitialized
        {
            get
            {
                return Id > 0 && !string.IsNullOrEmpty(Name);
            }
        }
    }

}