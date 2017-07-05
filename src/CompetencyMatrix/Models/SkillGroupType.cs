using System;
using System.Collections.Generic;

namespace CompetencyMatrix.Models
{
    public enum SkillGroupTypeQuality
    {
        Mandatory = 2,
        OneInGroup = 1,
        Optional = 0
    }

    public partial class SkillGroupType
    {
        public SkillGroupType()
        {
            PositionMatrixSkillGroup = new HashSet<PositionMatrixSkillGroup>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public bool IsDefault { get; set; }

        public SkillGroupTypeQuality Quality { get; set; }

        public virtual ICollection<PositionMatrixSkillGroup> PositionMatrixSkillGroup { get; set; }
    }
}
