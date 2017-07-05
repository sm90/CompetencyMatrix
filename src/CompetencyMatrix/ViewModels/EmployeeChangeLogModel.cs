using System;
using System.Collections.Generic;
using CompetencyMatrix.Models;
using System.Linq;

namespace CompetencyMatrix.ViewModels
{
    public class EmployeeChangeLogModel
    {
        //"<Skill name> skill has been added.Skill level is <Skill level>, Last used <last used date>"
        private const string SkillAdded = "{0} skill has been added. Skill level is {1}, Last used {2}";

        //"<Skill name> skill has been deleted.
        private const string SkillDeleted = "{0} skill has been deleted.";

        //Level of <Skill name> skill has been changed from <old skill level> to <new skill level>
        private const string SkillLevelHasBeenChanged = "Level of {0} skill has been changed from {1} to {2}";
        //	Last used date of <Skill name> skill has been changed from <old Last used date> to <new Last used date>
        private const string LastUsedDateHasBeenChanged = "	Last used date of {0} skill has been changed from {1} to {2}";

        public int EmployeeChangeLogId { get; set; }
        public DateTime LastChanged { get; set; }
        public string ActionDescription { get; set; }
        public string Status { get; set; }

        public string LastChangedString { get; set; }

        public static IList<EmployeeChangeLogModel> FromDbModel(List<ChangeLog> items, IQueryable<SkillLevelModel> skillLevel)
        {
            int? skillId;
            DateTime? lastUsedDate = null;
            string status = string.Empty;
            var model = new List<EmployeeChangeLogModel>();
            items.ForEach(a => {
                skillId = a.SkillId;
                var logItem = new EmployeeChangeLogModel();
                logItem.LastChangedString = a.When.ToString("yyyy-MM-dd");
                logItem.EmployeeChangeLogId = a.Id;

                if (a.Status == EmplyeeProfileStatus.Submitted)
                {
                    logItem.Status = "Not Approved";
                }
                else
                {
                    logItem.Status = a.Status.ToString();
                }


                if (a.Action == ChangeLogAction.Add)
                {
                    logItem.ActionDescription = string.Format(SkillAdded, a.Skill.Name, skillLevel.Where(x => x.Id == a.SkillLevelId).Single().Name, a.When.ToString("yyyy-MM-dd"));
                }
                else if (a.Action == ChangeLogAction.Delete)
                {
                    logItem.ActionDescription = string.Format(SkillDeleted, a.Skill.Name);
                }
                else if (a.OldSkillLevelId != null && a.SkillLevelId != a.OldSkillLevelId)
                {
                    logItem.ActionDescription = string.Format(SkillLevelHasBeenChanged, a.Skill.Name,
                        skillLevel.Where(x => x.Id == a.OldSkillLevelId).Single().Name,
                        skillLevel.Where(x => x.Id == a.SkillLevelId).Single().Name);
                }
                else if (a.OldSkillLevelId != null && a.SkillLevelId == a.OldSkillLevelId)
                {
                    logItem.ActionDescription = string.Format(LastUsedDateHasBeenChanged,
                        a.Skill.Name, !lastUsedDate.HasValue ? "Invalid lastUsedDate" : lastUsedDate.Value.ToString("yyyy-MM-dd"), a.When.ToString("yyyy-MM-dd"));
                }

                lastUsedDate = a.When;
                model.Add(logItem);
            });
            return model;
        }
    }
}
