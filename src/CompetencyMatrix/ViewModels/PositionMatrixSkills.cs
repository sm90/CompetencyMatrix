using System;
using System.Collections.Generic;
using System.Linq;
using CompetencyMatrix.Models;

namespace CompetencyMatrix.ViewModels
{
    public partial class PositionMatrixSkills
    {
        public List<PositionMatrixSkillGroup> Groups { get; set; }

        public List<ViewModels.PositionMatrixSkillListItem> Skills { get; set; }

        public int MatrixId { get; set; }

        public string MatrixName { get; set; }

        public string MatrixDescription { get; set; }

        public bool IsPublic { get; set; }

        public bool IsPublicEditable { get; set; } = true;

        protected static void AddMatrix(List<PositionMatrix> list, IEnumerable<PositionMatrixInheritance> matrixes)
        {
            foreach (var m in matrixes)
            {
                if (!list.Any(x => x.Id == m.ParentMatrix.Id))
                {
                    list.Add(m.ParentMatrix);
                }

                AddMatrix(list, m.ParentMatrix.PositionMatrixInheritanceMatrix);
            }
        }

        private static PositionMatrixSkillGroup FindGroup(PositionMatrixSkillGroup group, int id)
        {
            if (group.Id == id || group.OverridenGroupId.HasValue && group.OverridenGroupId.Value == id)
            {
                return group;
            }

            if (group.ChildGroups != null)
            {
                foreach (var childGroup in group.ChildGroups)
                {
                    var tmpGroup = FindGroup(childGroup, id);

                    if (tmpGroup != null)
                    {
                        return tmpGroup;
                    }
                }
            }

            return null;
        }

        private static void ProcessInheritedGroups(List<PositionMatrixSkillGroup> groups)
        {
            var groupsToRemove = new List<int>();

            for (int i = groups.Count - 1; i >= 0; i--)
            {
                var group = groups[i];

                if (group.OverridenGroupId.HasValue)
                {
                    var baseGroup = groups.FirstOrDefault(x => x.Id == group.OverridenGroupId.Value);

                    if (baseGroup != null)
                    {
                        //group.Skills.AddRange(baseGroup.Skills);
                        groupsToRemove.Add(baseGroup.Id);
                    }
                }

                if (group.Hidden)
                {
                    groupsToRemove.Add(group.Id);
                }
            }

            groups.RemoveAll(x => groupsToRemove.Contains(x.Id));
        }

        //Add groups which do not have any skills
        private static void AddMissedGroups(List<PositionMatrixSkillGroup> groups, ICollection<Models.PositionMatrixSkillGroup> emptyGroups)
        {
            foreach (var group in emptyGroups)
            {
                if (!groups.Any(x => x.Id == group.Id))
                {
                    groups.Add(PositionMatrixSkillGroup.FromDbModel(group, true));
                }                
            }
        }

        public static List<PositionMatrixSkill> GetSkillsForMatrix(PositionMatrix matrix)
        {
            var matrixChain = matrix.AllParents;
            matrixChain.Reverse();
            matrixChain.Add(matrix);

            AddMatrix(matrixChain, matrix.PositionMatrixInheritanceMatrix);
            
            var allSkills = new List<PositionMatrixSkill>();

            for (int i = 0; i < matrixChain.Count; i++)
            {
                allSkills.AddRange(matrixChain[i].PositionMatrixSkill);
            }

            return MergeSkills(allSkills, matrix);
        }

        public static PositionMatrixSkills FromDbModel(PositionMatrix matrix)
        {
            var result = new PositionMatrixSkills();

            result.IsPublic = matrix.IsPublic;
            result.MatrixDescription = matrix.Description;

            foreach (var parent in matrix.AllParents)
            {
                if (!parent.IsPublic)
                {
                    //If parent matrix is Private then its child can be private only.User can't change status from Private to Public for child if parent is Private.
                    result.IsPublicEditable = false;
                    break;
                }
            }

            if (result.IsPublicEditable && result.IsPublic)
            {
                foreach (var child in matrix.AllChildren)
                {
                    if (child.IsPublic)
                    {
                        //If public matrix has child, then we can not change the parent from public to private.
                        result.IsPublicEditable = false;
                        break;
                    }
                }
            }

            var combinedSkills = GetSkillsForMatrix(matrix);

            var groups = combinedSkills.GroupBy(e => e.SkillGroup);

            result.Groups = groups.Where(x => x.Key != null).Select(PositionMatrixSkillGroup.FromDbModel).OrderBy(e => e.GroupType.Id).ToList();

            AddMissedGroups(result.Groups, matrix.PositionMatrixSkillGroups);

            ProcessInheritedGroups(result.Groups);

            for (int i = result.Groups.Count - 1; i >= 0; i--)
            {
                ProcessSkills(result.Groups[i].Skills, matrix);
                //If this is a child group then add it to parent
                if (result.Groups[i].ParentGroupId.HasValue)
                {


                    for (int j = 0; j < result.Groups.Count; j++)
                    {
                        var parentGroup = FindGroup(result.Groups[j], result.Groups[i].ParentGroupId.Value);

                        if (parentGroup != null)
                        {
                            parentGroup.ChildGroups.Add(result.Groups[i]);

                            result.Groups.RemoveAt(i);
                            break;
                        }
                    }
                }
            }

            var groupsToRemove = new List<int>();
            foreach (var group in result.Groups)
            {
                if (group.Hidden)
                {
                    groupsToRemove.Add(group.Id);
                }
            }

            result.Groups.RemoveAll(x => groupsToRemove.Contains(x.Id));


            result.Skills = new List<PositionMatrixSkillListItem>();
            result.Skills.AddRange(combinedSkills.Select(PositionMatrixSkillListItem.FromDbModel)
                .Where(x => x.SkillGroupId == null).ToList());

            ProcessSkills(result.Skills, matrix);

            result.MatrixId = matrix.Id;
            result.MatrixName = matrix.Name;

            return result;
        }

        private static List<PositionMatrixSkill> MergeSkills(List<PositionMatrixSkill> allSkills, PositionMatrix currentMatrix)
        {
            var skills = new List<PositionMatrixSkill>();
            var grouppedSkills = allSkills.OrderByDescending(x => x.SkillGroupId).GroupBy(s => s.SkillId);


            foreach (var group in grouppedSkills)
            {
                bool isGroupSkill = false;

                if (group.Count() > 1)
                {
                    isGroupSkill = group.ElementAt(0).SkillGroupId.HasValue && group.ElementAt(1).SkillGroupId.HasValue;
                }

                foreach (var groupSkill in group)
                {
                    if (groupSkill.MatrixId == currentMatrix.Id)
                    {
                        var addedSkill = skills.FirstOrDefault(s => s.SkillId == groupSkill.SkillId);
                        if (addedSkill != null)
                            skills.Remove(addedSkill);
                        skills.Add(groupSkill);
                    }

                    if (!skills.Any(x => x.SkillId == groupSkill.SkillId))
                    {
                        skills.Add(groupSkill);
                        continue;
                    }

                    //If we at least 2 skills from groups then do not consirder root skills
                    if (isGroupSkill && groupSkill.SkillGroupId.HasValue)
                    {
                        UpdateSkill(skills, groupSkill, currentMatrix);
                    }
                    else if (!isGroupSkill)
                    {
                        //consider all skills
                        UpdateSkill(skills, groupSkill, currentMatrix);
                    }
                }
            }

            return skills;
        }

        private static void ProcessSkills(List<PositionMatrixSkillListItem> skills, PositionMatrix matrix)
        {
            foreach (var skill in skills)
            {
                //clear MatrixName for current matrix
                if (skill.MatrixId == matrix.Id)
                {
                    skill.MatrixName = string.Empty;
                }
                else
                {
                    skill.IsInherited = true;
                }
            }
        }

        private static SkillGroupTypeQuality GetGroupTag(PositionMatrixSkill skill)
        {
            SkillGroupTypeQuality quality = skill.SkillGroup.GroupType.Quality;

            return GetGroupTag(skill.SkillGroup, quality);
        }

        private static SkillGroupTypeQuality GetGroupTag(Models.PositionMatrixSkillGroup group, SkillGroupTypeQuality quality)
        {
            if (group.GroupType.Quality < quality)
            {
                quality = group.GroupType.Quality;

                if (group.ParentGroup != null)
                {
                    quality = GetGroupTag(group.ParentGroup, quality);
                }
            }

            return quality;
        }

        private static void UpdateSkill(List<PositionMatrixSkill> skills, PositionMatrixSkill skill, PositionMatrix currentMatrix)
        {
            var addedSkill = skills.First(x => x.SkillId == skill.SkillId);

            if (addedSkill.MatrixId != currentMatrix.Id)
            {
                if (addedSkill.SkillGroupId.HasValue && skill.SkillGroupId.HasValue)
                {
                    //Select skill from group which has higher Quality
                    if (GetGroupTag(skill) > GetGroupTag(addedSkill))
                    {
                        UpdateSkillInCollection(skills, skill);
                    }
                    //If groups have the same quality then Select skill  which has higher Quality 
                    else if (GetGroupTag(addedSkill) == GetGroupTag(skill) && skill.SkillLevel.Quality > addedSkill.SkillLevel.Quality)
                    {
                        UpdateSkillInCollection(skills, skill);
                    }
                }
                //If at least one skill from root group then Select skill  which has higher Quality 
                else if (skill.SkillLevel.Quality > addedSkill.SkillLevel.Quality)
                {
                    UpdateSkillInCollection(skills, skill);
                }

            }
        }

        private static void UpdateSkillInCollection(List<PositionMatrixSkill> skills, PositionMatrixSkill skill)
        {
            for (int i = 0; i < skills.Count; i++)
            {
                if (skills[i].SkillId == skill.SkillId)
                {
                    skills[i] = skill;
                    break;
                }
            }
        }
    }
}