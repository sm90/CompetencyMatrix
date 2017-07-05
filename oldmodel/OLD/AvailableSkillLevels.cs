using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompetencyMatrix.Models
{
    public partial class AvailableSkillLevels
    {
        public AvailableSkillLevels()
        {
            ChangeLogOldSkillLevel = new HashSet<ChangeLog>();
            ChangeLogSkillLevel = new HashSet<ChangeLog>();
            EmployeeMatrixSkills = new HashSet<EmployeeMatrixSkills>();
            PositionMatrixSkills = new HashSet<PositionMatrixSkills>();
            SkillLevelCriteria = new HashSet<SkillLevelCriteria>();
            SkillLevels = new HashSet<SkillLevels>();
        }

        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Column(TypeName = "ntext")]
        public string Description { get; set; }
        [Required]
        public int Quality { get; set; }

        [InverseProperty("OldSkillLevel")]
        public virtual ICollection<ChangeLog> ChangeLogOldSkillLevel { get; set; }
        [InverseProperty("SkillLevel")]
        public virtual ICollection<ChangeLog> ChangeLogSkillLevel { get; set; }
        [InverseProperty("SkillLevel")]
        public virtual ICollection<EmployeeMatrixSkills> EmployeeMatrixSkills { get; set; }
        [InverseProperty("SkillLevel")]
        public virtual ICollection<PositionMatrixSkills> PositionMatrixSkills { get; set; }
        [InverseProperty("AvailableSkillLevel")]
        public virtual ICollection<SkillLevelCriteria> SkillLevelCriteria { get; set; }
        [InverseProperty("AvailableSkillLevel")]
        public virtual ICollection<SkillLevels> SkillLevels { get; set; }
    }
}
