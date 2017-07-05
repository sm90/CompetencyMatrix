using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompetencyMatrix.Models
{
    public partial class ChangeLog
    {
        public int Id { get; set; }
        public int MatrixId { get; set; }
        public int SkillId { get; set; }
        public int SkillLevelId { get; set; }
        public int Action { get; set; }
        public int? OldSkillLevelId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime When { get; set; }
        public int ByWhom { get; set; }

        [ForeignKey("ByWhom")]
        [InverseProperty("ChangeLog")]
        public virtual Users ByWhomNavigation { get; set; }
        [ForeignKey("MatrixId")]
        [InverseProperty("ChangeLog")]
        public virtual EmployeeMatrix Matrix { get; set; }
        [ForeignKey("OldSkillLevelId")]
        [InverseProperty("ChangeLogOldSkillLevel")]
        public virtual AvailableSkillLevels OldSkillLevel { get; set; }
        [ForeignKey("SkillId")]
        [InverseProperty("ChangeLog")]
        public virtual Skills Skill { get; set; }
        [ForeignKey("SkillLevelId")]
        [InverseProperty("ChangeLogSkillLevel")]
        public virtual AvailableSkillLevels SkillLevel { get; set; }
    }
}
