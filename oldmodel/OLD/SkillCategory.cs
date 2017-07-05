using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompetencyMatrix.Models
{
    public partial class SkillCategory
    {
        public SkillCategory()
        {
            Skills = new HashSet<Skills>();
        }

        [Column("id")]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Column(TypeName = "ntext")]
        public string Description { get; set; }
        public int? ParentId { get; set; }

        [InverseProperty("Category")]
        public virtual ICollection<Skills> Skills { get; set; }
        [ForeignKey("ParentId")]
        [InverseProperty("ChildCategories")]
        public virtual SkillCategory Parent { get; set; }
        [InverseProperty("Parent")]
        public virtual ICollection<SkillCategory> ChildCategories { get; set; }
    }
}
