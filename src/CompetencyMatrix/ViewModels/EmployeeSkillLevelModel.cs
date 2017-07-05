using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CompetencyMatrix.ViewModels
{
    public class EmployeeSkillLevelModel
    {
        public int EmployeeId { get; set; }
        public int SkillId { get; set; }

        [Display(Name = "Skill")]
        public string SkillName { get; set; }
        [Display(Name = "Level")]
        public string LevelName { get; set; }
        [Display(Name = "Level")]
        public int LevelId { get; set; }
        public DateTime? LastUsed { get; set; }
        [Display(Name = "Last Used")]
        public string LastUsedYear { get; set; }
        public string LastUsedIso { get; set; }
        public string Description { get; set; }
        public List<SelectListItem> Levels { get; set; }

        public bool IsEdit { get; set; }
        public int ChangeLogId { get; set; }
    }
}
