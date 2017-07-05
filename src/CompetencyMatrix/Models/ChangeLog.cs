using System;
using System.Collections.Generic;

namespace CompetencyMatrix.Models
{
    public partial class ChangeLog
    {
        public int Id { get; set; }
        public int MatrixId { get; set; }
        public int SkillId { get; set; }
        public int SkillLevelId { get; set; }
        public ChangeLogAction Action { get; set; }

        public EmplyeeProfileStatus Status { get; set; }

        public int? OldSkillLevelId { get; set; }
        public DateTime When { get; set; }
        public string ByWhom { get; set; }

        public virtual AspNetUsers ByWhomNavigation { get; set; }
        public virtual EmployeeMatrix Matrix { get; set; }
        public virtual Skill Skill { get; set; }
    }

    public enum ChangeLogAction
    {
        Add = 1,
        Update = 2,
        Delete = 3
    }

}
