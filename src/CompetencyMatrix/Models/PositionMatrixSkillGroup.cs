using System;
using System.Collections.Generic;

namespace CompetencyMatrix.Models
{
    public partial class PositionMatrixSkillGroup
    {
        public PositionMatrixSkillGroup()
        {
            PositionMatrixSkill = new HashSet<PositionMatrixSkill>();
            Children = new List<PositionMatrixSkillGroup>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int GroupTypeId { get; set; }

        public int? ParentGroupId { get; set; }

        public PositionMatrixSkillGroup ParentGroup { get; set; }
        public List<PositionMatrixSkillGroup> Children { get; set; }

        public int? OverridenGroupId { get; set; }

        public int MatrixId { get; set; }

        public PositionMatrix Matrix { get; set; }

        public bool Hidden { get; set; }

        public virtual ICollection<PositionMatrixSkill> PositionMatrixSkill { get; set; }
        public virtual SkillGroupType GroupType { get; set; }
    }
}
