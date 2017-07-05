using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompetencyMatrix.Models
{
    public partial class Skills
    {
        public Skills()
        {
            ChangeLog = new HashSet<ChangeLog>();
            EmployeeMatrixSkills = new HashSet<EmployeeMatrixSkills>();
            PositionMatrixSkills = new HashSet<PositionMatrixSkills>();
            SkillCriteria = new HashSet<SkillCriteria>();
            SkillLevels = new HashSet<SkillLevels>();
        }

        [Column("id")]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Column(TypeName = "ntext")]
        public string Description { get; set; }
        public int CategoryId { get; set; }
        [Column(TypeName = "ntext")]
        public string TrainingMaterials { get; set; }
		[Column(TypeName = "ntext")]
		public string Questionarie { get; set; }

		[InverseProperty("Skill")]
        public virtual ICollection<ChangeLog> ChangeLog { get; set; }
        [InverseProperty("Skill")]
        public virtual ICollection<EmployeeMatrixSkills> EmployeeMatrixSkills { get; set; }
        [InverseProperty("Skill")]
        public virtual ICollection<PositionMatrixSkills> PositionMatrixSkills { get; set; }
        [InverseProperty("Skill")]
        public virtual ICollection<SkillCriteria> SkillCriteria { get; set; }
        [InverseProperty("Skill")]
        public virtual ICollection<SkillLevels> SkillLevels { get; set; }
        [ForeignKey("CategoryId")]
        [InverseProperty("Skills")]
        public virtual SkillCategory Category { get; set; }
    }
}
