using System.Collections.Generic;

namespace CompetencyMatrix.ViewModels
{
    public class EmployeeSkillStorageModel
    {
        public IList<EmployeeSkillModel> Skills { get; set; }
        public int EmployeeId { get; set; }
    }
}
