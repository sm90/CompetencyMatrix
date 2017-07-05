using System;
using System.Collections.Generic;
using System.Linq;
using CompetencyMatrix.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace CompetencyMatrix.ViewModels
{
    public class EmployeeSkillModel
    {
        // ID from EmployeeMatrixSkill 
        public int EmployeeSkillId { get; set; }
        public string SkillName { get; set; }
        public string LevelName { get; set; }

        [Display(Name = "Last Used")]
        public DateTime? LastUsed { get; set; }
        public string LastUsedYear { get; set; }
        public string Office { get; set; }
        // For support plain employee skill tree
        public int ChangeLogId { get; set; }
        public int LevelId { get; set; }
        public int SkillId { get; set; }
        public bool IsEdit { get; set; }

        public EntityState State { get; set; } = EntityState.Unchanged;

        public int OldLevelId { get; set; }

        public static IList<EmployeeSkillModel> FromDbModel(List<EmployeeMatrixSkill> items,  IList<SkillLevelModel> levels, int matrixId)
        {
            IList<EmployeeSkillModel> model = new List<EmployeeSkillModel>();
            
            items.ForEach(a =>
            {
                EmployeeSkillModel item = new EmployeeSkillModel();
                item.LevelName = levels.First(x => x.Id == a.SkillLevelId).Name;
                item.SkillName = a.Skill.Name;
                item.SkillId = a.Skill.Id;
                item.EmployeeSkillId = a.Id;
                item.LevelId = a.SkillLevelId;
                item.ChangeLogId = 0; //TODO possibly should be int?
                item.State = a.Id == 0 ? EntityState.Added : EntityState.Unchanged; 
                if (a.Skill.ChangeLog != null && a.Skill.ChangeLog.Any())
                {
                    item.LastUsed = a.Skill.ChangeLog.OrderByDescending(x => x.When).First().When;
                    item.LastUsedYear = a.Skill.ChangeLog.OrderByDescending(x => x.When).First().When.Year.ToString();
                    model.Add(item);
                }
                else
                {
                    model.Add(item);
                }

            });
            return model;
        }

        public static IList<EmployeeSkillModel> FromDbModelWithLogData(List<EmployeeMatrixSkill> items, IList<SkillLevelModel> levels, int matrixId)
        {
            IList<EmployeeSkillModel> model = new List<EmployeeSkillModel>();

            items.ForEach(a =>
            {
                EmployeeSkillModel item = new EmployeeSkillModel();
                item.LevelName = a.SkillLevel.Name;
                item.SkillName = a.Skill.Name;
                item.SkillId = a.Skill.Id;
                item.EmployeeSkillId = a.Id;
                item.LevelId = a.SkillLevel.Id;
                item.ChangeLogId = 0; //TODO possibly should be int?

                if (a.Id == 0)
                {
                    item.State = EntityState.Added;
                }

                if (a.Skill.ChangeLog != null)
                {

                    foreach (var d in a.Skill.ChangeLog.Where(b => b.MatrixId == matrixId))
                    {
                        if (item == null)
                            item = new EmployeeSkillModel();
                        item.SkillName = a.Skill.Name;
                        item.LevelName = levels.ToList().First(i => i.Id == d.SkillLevelId).Name;
                        item.LevelId = levels.ToList().First(i => i.Id == d.SkillLevelId).Id;
                        item.LastUsed = d.When;
                        item.EmployeeSkillId = a.Id;
                        item.LastUsedYear = d.When.Year.ToString();
                        item.ChangeLogId = d.Id;
                        item.SkillId = d.SkillId;
                        model.Add(item);
                        item = null;
                    }
                }
                else
                {
                    model.Add(item);
                }

            });
            return model;
        }

        public static List<EmployeeSkillModel> FromChangeLogDbModel(IList<ChangeLog> items)
        {
            var skills = new List<EmployeeSkillModel>();

            foreach (var changeLog in items)
            {
                skills.Add(new EmployeeSkillModel()
                {
                    EmployeeSkillId = changeLog.SkillId,
                    SkillId = changeLog.SkillId,
                    LevelId = changeLog.SkillLevelId,
                    LastUsed = changeLog.When,
                });
            }

            return skills;
        }

    }
}
