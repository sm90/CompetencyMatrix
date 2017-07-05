using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompetencyMatrix.Models
{
    public partial class EmployeeMatrixSkills
    {
        [Column("id")]
        public int Id { get; set; }
        public int MatrixId { get; set; }
        public int SkillId { get; set; }
        public int SkillLevelId { get; set; }

        [ForeignKey("MatrixId")]
        [InverseProperty("EmployeeMatrixSkills")]
        public virtual EmployeeMatrix Matrix { get; set; }
        [ForeignKey("SkillId")]
        [InverseProperty("EmployeeMatrixSkills")]
        public virtual Skills Skill { get; set; }
        [ForeignKey("SkillLevelId")]
        [InverseProperty("EmployeeMatrixSkills")]
        public virtual AvailableSkillLevels SkillLevel { get; set; }
    }
}
