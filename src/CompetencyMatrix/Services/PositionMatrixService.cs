using System;
using CompetencyMatrix.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using CompetencyMatrix.ViewModels;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data.Common;

namespace CompetencyMatrix.Services
{
    public class PositionMatrixService : IPositionMatrixService
    {
        private readonly ICompetencyMatrixContext _dbContext;
        private readonly IPositionMatrixInheritanceService _inheritanceService;

        public PositionMatrixService(ICompetencyMatrixContext dbContext)
        {
            _dbContext = dbContext;
            _inheritanceService = new PositionMatrixInheritanceService(dbContext);
        }

        public PositionMatrix GetFullPositionMatrixbyId(int positionMatrixId)
        {
            var currentMatrix = _dbContext.PositionMatrix.Include(pm => pm.PositionMatrixInheritanceMatrix)
                .Include(pm => pm.PositionMatrixInheritanceParentMatrix)
                .Include(pm => pm.Owner)
                .SingleOrDefault(x => x.Id == positionMatrixId);

            _inheritanceService.PopulateAncestors(currentMatrix);
            _inheritanceService.PopulateDescendants(currentMatrix);

            return currentMatrix;
        }

        public void SetParentMatrixes(PositionMatrixInheritanceManagementViewModel viewModel)
        {
            var positionMatrix = _dbContext.PositionMatrix
                .Include(pm => pm.PositionMatrixInheritanceMatrix)
                .ThenInclude(x => x.ParentMatrix)
                .ToList()
                .Single(e => e.Id == viewModel.CurrentMatrix.Id);

            var parentMatrixIds = viewModel.ParentMatrixes.Select(m => m.Id).ToList();

            var matrixesToAddIds = parentMatrixIds
                .Where(x => !positionMatrix.PositionMatrixInheritanceMatrix.Select(i => i.ParentMatrixId).Contains(x))
                .ToList();
            var matrixInheritancesToRemove = positionMatrix.PositionMatrixInheritanceMatrix
                .Where(x => !parentMatrixIds.Contains(x.ParentMatrixId))
                .ToList();

            foreach (var parentMatrixId in matrixesToAddIds)
            {
                positionMatrix.PositionMatrixInheritanceMatrix.Add(new PositionMatrixInheritance
                {
                    Matrix = positionMatrix,
                    ParentMatrixId = parentMatrixId
                });
            }

            foreach (var inheritance in matrixInheritancesToRemove)
            {
                if (inheritance.ParentMatrixId > 0)
                {
                    //Remove hidden groups
                    var groups = _dbContext.PositionMatrixSkillGroup.Where(x => x.MatrixId == inheritance.ParentMatrixId);

                    var groupsToDelete = _dbContext.PositionMatrixSkillGroup
                            .Where(x => groups.Any(g => x.OverridenGroupId == g.Id) && x.Hidden).ToList();

                    _dbContext.PositionMatrixSkillGroup.RemoveRange(groupsToDelete);

                    //Remove hidden skills
                    var skills = _dbContext.PositionMatrixSkill.Where(x => !x.SkillGroupId.HasValue && x.MatrixId == inheritance.ParentMatrixId);
                    var skillsToDelete = _dbContext.PositionMatrixSkill.Where(x => !x.SkillGroupId.HasValue
                                                    && x.Hidden
                                                    && x.MatrixId == inheritance.MatrixId
                                                    && skills.Any(s => s.SkillId == x.SkillId))
                                                    .ToList();
                    _dbContext.PositionMatrixSkill.RemoveRange(skillsToDelete);
                }

                _dbContext.Entry(inheritance).State = EntityState.Deleted;
            }

            _dbContext.SaveChanges();
        }

        public PositionMatrix Create(PositionMatrixDetails viewModel)
        {
            var createdPositionMatrix = new PositionMatrix
            {
                Name = viewModel.Name,
                Description = viewModel.Description,
                IsPublic = false,
                OwnerId = viewModel.Owner
            };

            _dbContext.PositionMatrix.Add(createdPositionMatrix);
            _dbContext.SaveChanges();

            return createdPositionMatrix;
        }

        public void DeleteMatrix(int positionMatrixId)
        {
            var matrix = _dbContext.PositionMatrix
                .Include(m => m.PositionMatrixInheritanceParentMatrix)
                .Include(m => m.PositionMatrixInheritanceMatrix)
                .SingleOrDefault(x => x.Id == positionMatrixId);

            if (matrix == null) return;
            if (matrix.PositionMatrixInheritanceParentMatrix.Count > 0) return; //Cannot delete matrix with child

            matrix.Name = matrix.Name + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            matrix.PositionMatrixInheritanceParentMatrix
                .Union(matrix.PositionMatrixInheritanceMatrix)
                .ToList()
                .ForEach(inh => _dbContext.Entry(inh).State = EntityState.Deleted);

            matrix.IsDeleted = true;
            _dbContext.SaveChanges();
        }

        public PositionMatrix GetPositionMatrix(int positionMatrixId)
        {
            var positionMatrix = _dbContext.PositionMatrix
                .Include(pm => pm.PositionMatrixInheritanceMatrix)
                .Include(pm => pm.PositionMatrixInheritanceParentMatrix)
                .ToList()
                .SingleOrDefault(e => e.Id == positionMatrixId);

            var positionMatrixSkills =
                _dbContext.PositionMatrixSkill.Include(pms => pms.SkillGroup)
                    .ThenInclude(e => e.GroupType)
                    .Include(pms => pms.Skill)
                    .Include(pms => pms.SkillLevel).ToList();

            var parentMatrixes = PositionMatrixList.FromDbModel(
                    positionMatrix.PositionMatrixInheritanceMatrix.Select(p => p.ParentMatrix).Where(x => !x.IsDeleted));

            positionMatrix.PositionMatrixSkillGroups = _dbContext.PositionMatrixSkillGroup
                .Include(x => x.PositionMatrixSkill)
                .Select(x => x)
                .Where (x => x.MatrixId == positionMatrixId || parentMatrixes.Any(p => p.Id == x.MatrixId))
                .ToList();

            _inheritanceService.PopulateAncestors(positionMatrix);
            _inheritanceService.PopulateDescendants(positionMatrix);

            return positionMatrix;
        }


        public PositionMatrixSkills GetPositionMatrixSkills(int positionMatrixId)
        {
            var positionMatrix = GetPositionMatrix(positionMatrixId);

            var data = PositionMatrixSkills.FromDbModel(positionMatrix);

            return data;
        }

        /// <summary>
        /// Returns data for added Skill or group
        /// </summary>
        /// <param name="id"></param>
        /// <param name="parentId"></param>
        /// <param name="matrixId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public ViewModels.PositionMatrixSkillGroup GetMatrixSkillData(int id, int? parentId, int matrixId,
            SkillViewType type)
        {
            var groupTypeId = GetDefaultSkillGroupType().Id;

            var matrix = GetPositionMatrix(matrixId);

            var viewModel = new ViewModels.PositionMatrixSkillGroup()
            {
                SkillViewType = type,
                GroupTypeId = groupTypeId,
                ParentGroupId = parentId
            };


            if (type == SkillViewType.Skill)
            {
                var skill = _dbContext.Skill
                    .Include(x => x.EvaluationModel)
                    .ThenInclude(x => x.SkillEvaluationModelLevel)
                    .Include(x => x.SkillCriteria)
                    .Where(x => x.Id == id)
                    .First();

                viewModel.Skills = new List<ViewModels.PositionMatrixSkillListItem>();

                var model = PositionMatrixSkillListItem.FromDbModel(skill);

                model.Leaf = type == SkillViewType.Skill;

                model.SkillGroupId = parentId;

                if (!model.SkillGroupId.HasValue)
                {
                    model.SkillGroupTypeId = groupTypeId;
                }

                model.MatrixId = matrixId;

                model.State = EntityState.Added;
                model.Id = -model.Id;

                viewModel.Skills.Add(model);
            }
            else
            {
                var model = _dbContext.SkillCategory
                    .Where(x => x.Id == id)
                    .Include(x => x.Skill)
                    .ThenInclude(x => x.EvaluationModel)
                    .ThenInclude(x => x.SkillEvaluationModelLevel)
                    .First();

                viewModel.Name = model.Name;
                viewModel.Id = -id;
                viewModel.Skills = new List<PositionMatrixSkillListItem>();

                //TODO: fetch child categories
                //foreach (var skill in model.Skill)
                //{
                //    var listItem = PositionMatrixSkillListItem.FromDbModel(skill);
                //    viewModel.Skills.Add(listItem);
                //}

                AddSkills(model, viewModel.Skills, matrix);
            }
            return viewModel;
        }

        private void AddSkills(SkillCategory category, ICollection<PositionMatrixSkillListItem> skills,
            PositionMatrix matrix)
        {
            foreach (var skill in category.Skill)
            {
                var listItem = PositionMatrixSkillListItem.FromDbModel(skill);
                listItem.MatrixId = matrix.Id;
                listItem.State = EntityState.Added;

                skills.Add(listItem);
            }

            //if (category.Parent != null)
            //{
            //    AddSkills(category.Parent, skills, matrix);
            //}
        }

        public SkillGroupType GetDefaultSkillGroupType()
        {
            return _dbContext.SkillGroupType.First(x => x.IsDefault);
        }

        #region Update Matrix

        private void ProcessInheritedSkills(ViewModels.PositionMatrixSkillGroup group, PositionMatrixSkills positionMatrix)
        {
            for (int i = group.Skills.Count - 1; i >= 0; i--)
            {
                var skill = group.Skills[i];
                //
                //if ((skill.MatrixId != positionMatrix.MatrixId && skill.State == EntityState.Unchanged) || skill.State == EntityState.Unchanged)
                //{
                //    group.Skills.RemoveAt(i);
                //}
                //else if (skill.MatrixId != positionMatrix.MatrixId && skill.State == EntityState.Modified)
                //{
                //    skill.MatrixId = positionMatrix.MatrixId;
                //    skill.State = EntityState.Added;
                //}

                //when we change group then break relationship for all skills
                if (skill.MatrixId != positionMatrix.MatrixId)
                {
                    skill.MatrixId = positionMatrix.MatrixId;


                    if (skill.State != EntityState.Deleted)
                    {
                        skill.State = EntityState.Added;
                    }
                    else
                    {
                        group.Skills.RemoveAt(i);
                    }
                }
            }
        }

        private bool RequireNewGroup(ViewModels.PositionMatrixSkillGroup group, PositionMatrixSkills positionMatrix,
            List<ViewModels.PositionMatrixSkillGroup> groupsToAdd)
        {
            //TODO: add support of childgroups

            if (group.MatrixId != positionMatrix.MatrixId)
            {
                for (int i = 0; i < group.Skills.Count; i++)
                {
                    switch (group.Skills[i].State)
                    {
                        case EntityState.Modified:
                        case EntityState.Deleted:
                        case EntityState.Added:

                            return true;
                        default:
                            break;
                    }
                }
            }

            return false;
        }

        private void OverrideGroup(ViewModels.PositionMatrixSkillGroup group, PositionMatrixSkills positionMatrix)
        {
            group.OverridenGroupId = group.Id;
            group.MatrixId = positionMatrix.MatrixId;
            group.Id = 0;
            group.State = EntityState.Added;
            group.Name = group.Name;

            foreach (var childGroup in group.ChildGroups)
            {
                if (childGroup.State != EntityState.Deleted)
                {
                    if (childGroup.MatrixId != positionMatrix.MatrixId)
                    {
                        childGroup.Name = childGroup.Name;
                        childGroup.ParentGroupId = 0;
                        childGroup.OverridenGroupId = childGroup.Id;
                        childGroup.MatrixId = positionMatrix.MatrixId;
                        childGroup.State = EntityState.Added;

                        OverrideGroup(childGroup, positionMatrix);
                    }
                }
            }

            ProcessInheritedSkills(group, positionMatrix);
        }

        private void ProcessGroups(List<ViewModels.PositionMatrixSkillGroup> groupsToAdd,
            List<ViewModels.PositionMatrixSkillGroup> groupsToModify,
            List<ViewModels.PositionMatrixSkillGroup> groupsToDelete, int matrixId)
        {
            if (groupsToDelete.Any())
            {
                DeleteBindedGroups(groupsToDelete, matrixId);

                var deleteGroups = _dbContext.PositionMatrixSkillGroup.Select(x => x).Where(x => groupsToDelete.Any(d => d.Id == x.Id));

                foreach (var group in deleteGroups)
                {
                    group.OverridenGroupId = null;
                    group.ParentGroupId = null;
                    _dbContext.PositionMatrixSkillGroup.Update(group);
                }

                _dbContext.SaveChanges();

                _dbContext.PositionMatrixSkillGroup.RemoveRange(deleteGroups);

            }
            foreach (var group in groupsToAdd)
            {
                var model = ViewModels.PositionMatrixSkillGroup.ToDbModel(group, matrixId, _dbContext);

                _dbContext.PositionMatrixSkillGroup.Add(model);
            }

            foreach (var group in groupsToModify)
            {
                var editedGroup = _dbContext.PositionMatrixSkillGroup.First(x => x.Id == group.Id);

                editedGroup.Name = group.Name;
                editedGroup.GroupTypeId = group.GroupTypeId;
                editedGroup.Hidden = group.Hidden;

                _dbContext.PositionMatrixSkillGroup.Update(editedGroup);
            }

        }

        private void ProcessSkills(List<PositionMatrixSkillListItem> skillsToAdd, List<PositionMatrixSkillListItem> skillsToModify)
        {
            foreach (var item in skillsToAdd)
            {
                _dbContext.PositionMatrixSkill.Add(PositionMatrixSkillListItem.ToDbModel(item));
            }

            foreach (var skill in skillsToModify)
            {
                var dbSkill = _dbContext.PositionMatrixSkill.First(x => x.Id == skill.Id);

                dbSkill.SkillGroupTypeId = skill.SkillGroupTypeId;
                dbSkill.SkillLevelId = skill.SkillLevelId;
                dbSkill.Hidden = skill.Hidden;

                _dbContext.PositionMatrixSkill.Update(dbSkill);
            }
        }

        private void ProcessGroup(ViewModels.PositionMatrixSkillGroup group, PositionMatrixSkills positionMatrix,
            List<ViewModels.PositionMatrixSkillGroup> groupsToAdd,
            List<ViewModels.PositionMatrixSkillGroup> groupsToModify,
            List<ViewModels.PositionMatrixSkillGroup> groupsToDelete,
            List<PositionMatrixSkillListItem> skillsToAdd, List<PositionMatrixSkillListItem> skillsToModify,
            List<PositionMatrixSkillListItem> skillsToDelete)
        {
            if (group.State == EntityState.Modified)
            {
                //Group modified in the current matrix, if matrix is inherited then create new skill                            
                if (group.MatrixId == positionMatrix.MatrixId)
                {
                    groupsToModify.Add(group);
                }
                else
                {
                    //Group modified in the inherited matrix so create new group for this matrix
                    OverrideGroup(group, positionMatrix);
                    groupsToAdd.Add(group);
                }
            }
            else if (group.State == EntityState.Added)
            {
                groupsToAdd.Add(group);
            }
            else if (group.State == EntityState.Deleted)
            {
                if (group.MatrixId != positionMatrix.MatrixId)
                {
                    group.OverridenGroupId = group.Id;
                    group.MatrixId = positionMatrix.MatrixId;
                    group.Id = 0;
                    group.Hidden = true;
                    group.Skills.Clear();
                    groupsToAdd.Add(group);
                }
                else
                {
                    if (group.OverridenGroupId > 0)
                    {
                        group.Hidden = true;
                        groupsToModify.Add(group);
                    }
                    else
                    {
                        groupsToDelete.Add(group);
                    }
                    

                    foreach (var childGroup in group.ChildGroups)
                    {
                        childGroup.State = EntityState.Deleted;
                    }

                    foreach (var skill in group.Skills)
                    {
                        if (skill.MatrixId == positionMatrix.MatrixId)
                        {
                            skillsToDelete.Add(skill);
                        }
                    }
                }
            }
            else if (RequireNewGroup(group, positionMatrix, groupsToAdd))
            {
                OverrideGroup(group, positionMatrix);
                groupsToAdd.Add(group);
            }

            if (group.State != EntityState.Deleted && group.State != EntityState.Added)
            {
                foreach (var skill in group.Skills)
                {
                    if (skill.State == EntityState.Modified)
                    {
                        if (skill.MatrixId == positionMatrix.MatrixId)
                        {
                            //Skill modified in the current matrix, if matrix is inherited then create new skill                            
                            skillsToModify.Add(skill);
                        }
                        else
                        {
                            skill.Id = 0;
                            skill.MatrixId = positionMatrix.MatrixId;
                            skill.SkillGroupId = group.Id;
                            if (group.State != EntityState.Added)
                            {
                                skillsToAdd.Add(skill);
                            }
                        }
                    }
                    else if (skill.State == EntityState.Added && group.State != EntityState.Added)
                    {
                        var hiddenSkill = group.Skills.FirstOrDefault(x => x.SkillId == skill.SkillId && x.Hidden);

                        if (hiddenSkill == null)
                        {
                            skill.Id = 0;
                            skill.MatrixId = positionMatrix.MatrixId;

                            if (group.Id > 0)
                            {
                                skill.SkillGroupId = group.Id;
                                skillsToAdd.Add(skill);
                            }
                        }
                        else
                        {
                            if (skill.MatrixId == positionMatrix.MatrixId)
                            {
                                hiddenSkill.Hidden = false;
                                hiddenSkill.SkillLevelId = skill.SkillLevelId;
                                hiddenSkill.SkillGroupTypeId = skill.SkillGroupTypeId;
                                skillsToModify.Add(hiddenSkill);
                            }
                        }
                    }
                    else if (skill.State == EntityState.Deleted)
                    {
                        if (group.State == EntityState.Added)
                        {
                            //we hide inherited skill
                            skill.Id = 0;
                            skill.Hidden = true;
                            skill.MatrixId = positionMatrix.MatrixId;
                            //skill.SkillGroupId = 0;
                        }
                        else if (skill.MatrixId != positionMatrix.MatrixId)
                        {
                            //we hide inherited skill for existed group
                            skill.Id = 0;
                            skill.Hidden = true;
                            skill.MatrixId = positionMatrix.MatrixId;
                            skill.SkillGroupId = group.Id;
                            skillsToAdd.Add(skill);
                        }
                        else
                        {
                            skillsToDelete.Add(skill);
                            //if (skill.IsInherited)
                            //{
                            //    //for group which overrides other just hide skill
                            //    skill.Hidden = true;
                            //    skillsToModify.Add(skill);
                            //}
                            //else
                            //{
                            //    skillsToDelete.Add(skill);
                            //}                            
                        }
                    }
                }
            }

            if (group.State != EntityState.Added)
            {
                foreach (var childGroup in group.ChildGroups)
                {
                    ProcessGroup(childGroup, positionMatrix, groupsToAdd, groupsToModify, groupsToDelete, skillsToAdd,
                        skillsToModify, skillsToDelete);
                }
            }
        }

        private void DeleteBindedGroups(List<ViewModels.PositionMatrixSkillGroup> groups, int matrixId)
        {
            foreach (var groupToDel in groups)
            {


                var bindedGroups = _dbContext.PositionMatrixSkillGroup
                                        .Select(x => x)
                                        .Where(x => (x.ParentGroupId == groupToDel.Id || x.OverridenGroupId == groupToDel.Id) && x.MatrixId != matrixId)
                                        .ToList();

                foreach (var group in bindedGroups)
                {
                    if (group.ParentGroupId == groupToDel.Id)
                    {
                        group.ParentGroupId = null;
                    }

                    if (group.OverridenGroupId == groupToDel.Id)
                    {
                        group.OverridenGroupId = null;
                    }

                    if (group.MatrixId != matrixId)
                    {
                        _dbContext.PositionMatrixSkillGroup.Update(group);
                    }
                }
            }
        }

        public bool Update(PositionMatrixSkills positionMatrix)
        {
            var skillsToAdd = new List<PositionMatrixSkillListItem>();
            var skillsToModify = new List<PositionMatrixSkillListItem>();
            var skillsToDelete = new List<PositionMatrixSkillListItem>();
            var groupsToAdd = new List<ViewModels.PositionMatrixSkillGroup>();
            var groupsToModify = new List<ViewModels.PositionMatrixSkillGroup>();
            var groupsToDelete = new List<ViewModels.PositionMatrixSkillGroup>();

            var matrix = _dbContext.PositionMatrix.First(x => x.Id == positionMatrix.MatrixId);

            matrix.Name = System.Net.WebUtility.HtmlEncode(positionMatrix.MatrixName);
            matrix.IsPublic = positionMatrix.IsPublic;

            _dbContext.PositionMatrix.Update(matrix);

            if (positionMatrix.Skills != null)
            {
                foreach (var skill in positionMatrix.Skills)
                {
                    if (skill.State == EntityState.Added
                        || (skill.MatrixId != positionMatrix.MatrixId && skill.State == EntityState.Modified))
                    {
                        var hiddenSkill = positionMatrix.Skills.FirstOrDefault(x => x.SkillId == skill.SkillId && x.Hidden);

                        if (hiddenSkill == null)
                        {
                            skill.SkillGroupId = null;
                            skill.MatrixId = positionMatrix.MatrixId;
                            skill.State = EntityState.Added;
                            skillsToAdd.Add(skill);
                        }
                        else
                        {
                            hiddenSkill.Hidden = false;
                            hiddenSkill.SkillGroupTypeId = skill.SkillGroupTypeId;
                            hiddenSkill.SkillLevelId = skill.SkillLevelId;
                            skillsToModify.Add(hiddenSkill);
                        }
                    }
                    else if (skill.State == EntityState.Deleted)
                    {
                        if (skill.MatrixId != positionMatrix.MatrixId)
                        {
                            skill.Id = 0;
                            skill.MatrixId = positionMatrix.MatrixId;
                            skill.Hidden = true;

                            skillsToAdd.Add(skill);
                        }
                        else
                        {
                            if (skill.IsInherited)
                            {
                                skill.Hidden = true;
                                skillsToModify.Add(skill);
                            }
                            else
                            {
                                skillsToDelete.Add(skill);
                            }

                            
                        }
                    }
                    else if (skill.State == EntityState.Modified)
                    {
                        skillsToModify.Add(skill);
                    }
                }
            }
            
            CheckChangesInGroups(positionMatrix);
            
            foreach (var group in positionMatrix.Groups)
            {
                ProcessGroup(group, positionMatrix, groupsToAdd, groupsToModify, groupsToDelete, skillsToAdd, skillsToModify, skillsToDelete);
            }


            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {

                    foreach (var skill in skillsToDelete)
                    {
                        var dbSkill = new PositionMatrixSkill() { Id = skill.Id };

                        _dbContext.PositionMatrixSkill.Remove(dbSkill);
                    }

                    //save deleted skills to prevent foreign key references
                    _dbContext.SaveChanges();

                    ProcessGroups(groupsToAdd, groupsToModify, groupsToDelete, positionMatrix.MatrixId);

                    ProcessSkills(skillsToAdd, skillsToModify);

                    _dbContext.SaveChanges();

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
            }
            return true;
        }

        /// <summary>
        ///  When editing anything for subgroup in inherited matix - connection 
        ///  should be removed both for subgroup and group.
        /// </summary>
        /// <param name="positionMatrix"></param>
        private void CheckChangesInGroups(PositionMatrixSkills positionMatrix)
        {
            for(int i = 0; i < positionMatrix.Groups.Count; i++)
            {
                for(int g = 0; g < positionMatrix.Groups[i].ChildGroups.Count; g++)
                {
                    if(positionMatrix.Groups[i].ChildGroups[g].State == EntityState.Modified)
                    {
                        positionMatrix.Groups[i].State = EntityState.Modified;
                        break;
                    }
                }
            }
        }


        #endregion
    }
}