using CompetencyMatrix.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompetencyMatrix.ViewModels
{
    public class EmployeePositionSkillsGaps : PositionMatrixSkills
    {
        public new List<EmployeePositionMatrixGapsGroup> Groups { get; set; }

        public List<Models.EmployeeGapModel> Gaps { get; set; }

        public static EmployeePositionSkillsGaps FromDbModel(PositionMatrix matrix, Employee employee)
        {
            var result = new EmployeePositionSkillsGaps();
            var combinedSkills = PositionMatrixSkills.GetSkillsForMatrix(matrix);
            var groups = combinedSkills.GroupBy(e => e.SkillGroup);

            result.Groups = groups
                .Where(x => x.Key != null)
                .Select(g => EmployeePositionMatrixGapsGroup.FromDbModel(g, employee)).OrderBy(e => e.GroupType.Id).ToList();

            result.MatrixId = matrix.Id;
            result.MatrixName = matrix.Name;

            result.Gaps = new List<EmployeeGapModel>();
            var rootSkills = combinedSkills.Where(s => s.SkillGroup == null).ToList();
            foreach (var dbSkill in rootSkills)
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
