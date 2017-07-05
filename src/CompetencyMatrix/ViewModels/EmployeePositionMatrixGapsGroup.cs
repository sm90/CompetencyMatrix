using CompetencyMatrix.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CompetencyMatrix.ViewModels
{
    public class EmployeePositionMatrixGapsGroup : PositionMatrixSkillGroup
    {
        public EmployeePositionMatrixGapsGroup()
        {
            Gaps = new List<EmployeeGapModel>();
        }

        public virtual ICollection<EmployeeGapModel> Gaps { get; set; }

        public static EmployeePositionMatrixGapsGroup FromDbModel(IGrouping<Models.PositionMatrixSkillGroup, Models.PositionMatrixSkill> group, Employee employee)
        {
            var result = new EmployeePositionMatrixGapsGroup();

            result.Id = group.Key.Id;
            result.Name = group.Key.Name;
            result.GroupTypeId = group.Key.GroupType.Id;
            result.GroupType = SkillGroupTypeViewModel.FromDbModel(group.Key.GroupType);

            result.Gaps = new List<EmployeeGapModel>();
            foreach (var dbSkill in group)
            {
                var listItem = new EmployeeGapModel()
                {
                    PositionSkill = dbSkill,
                    EmployeeSkill = employee.Matrix.EmployeeMatrixSkill.Where(x => x.SkillId == dbSkill.SkillId).FirstOrDefault()
                };

                result.Gaps.Add(listItem);
            }

            return result;
        }
    }
}
