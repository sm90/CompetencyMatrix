using System.Collections.Generic;
using CompetencyMatrix.ViewModels;

namespace CompetencyMatrix.Infrastructure
{
    public interface IServerVariables
    {
        int CurrentUserId { get; set; }
        int CurrentEditUserId { get; set; }
        string CurrentAspUserId { get; set; }
        string CurrentUserPosition { get; set; }
        EmployeeSkillStorageModel CurrentUserSkills { get; set; }
    }
}