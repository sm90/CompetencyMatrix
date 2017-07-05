using CompetencyMatrix.Models;
using CompetencyMatrix.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompetencyMatrix.Services
{
    public interface IEmployeeService
    {
        void SaveEmployee(int employeeId, EmplyeeProfileStatus status);

        List<EmployeeMatrixSkill> GetTransactionSkills(int matrixId);

        EmployeeSkillStorageModel GetEmployeeSkillStorageModel(int employeeId);

        void ApproveEmployee(int id, int approverId);

        void RejectEmployee(int id, int approverId);

        void CancelEmployeeTransaction(int id);
    }
}
