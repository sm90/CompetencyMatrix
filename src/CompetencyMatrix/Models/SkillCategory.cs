using System;
using System.Collections.Generic;

namespace CompetencyMatrix.Models
{
    public partial class SkillCategory
    {
        public SkillCategory()
        {
            Skill = new HashSet<Skill>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? ParentId { get; set; }

        public virtual ICollection<Skill> Skill { get; set; }
        public virtual SkillCategory Parent { get; set; }
        public virtual ICollection<SkillCategory> InverseParent { get; set; }
    }
}
