using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompetencyMatrix.Models
{
    public partial class PositionMatrixSkill
    {
        public int Id { get; set; }
        public int MatrixId { get; set; }
        public int SkillId { get; set; }
        public int SkillLevelId { get; set; }
        public int? SkillGroupId { get; set; }
        public int? SkillGroupTypeId { get; set; }
        public bool Hidden { get; set; }


        public virtual PositionMatrix Matrix { get; set; }
        public virtual PositionMatrixSkillGroup SkillGroup { get; set; }
        public virtual Skill Skill { get; set; }
        public virtual SkillLevelModel SkillLevel { get; set; }

        [NotMapped]
        public bool IsInherited { get; set; }
    }
}
