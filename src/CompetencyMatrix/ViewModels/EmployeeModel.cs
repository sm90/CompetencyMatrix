using CompetencyMatrix.Infrastructure;
using CompetencyMatrix.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompetencyMatrix.ViewModels
{
    public class EmployeeModel
    {
        public int Id { get; set; }
        public int MatrixId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public byte[] Photo { get; set; }
        public string ProfileStatus { get; set; }

        public EmplyeeProfileStatus Status { get; set; }
        public string Email { get; set; }
        public int? Manager { get; set; }
        public string Skype { get; set; }
        public string Cell { get; set; }
        public string Office { get; set; }

        public EmployeeMatrixApproval MatrixApproval { get; set; }

        public Employee ManagerNavigation { get; set; }

        public ICollection<Employee> InverseManagerNavigation { get; set; }

        public static EmployeeModel FromDbModel(Employee employee, int currentUserId)
        {
            var result = new EmployeeModel();

            result.Id = employee.Id;
            result.MatrixId = employee.MatrixId;
            result.Name = employee.Name;
            result.Title = employee.Title;

            result.ProfileStatus = employee.ProfileStatus.ToString();
            result.MatrixApproval = employee.MatrixApproval;

            result.Email = employee.Email;
            result.Manager = employee.Manager;
            result.ManagerNavigation = employee.ManagerNavigation;
            result.Skype = employee.Skype;
            result.Cell = employee.Cell;
            result.Office = employee.Office;

            result.Status = employee.ProfileStatus;

            result.ProfileStatus = GetProfileStatus(result, currentUserId);

            result.InverseManagerNavigation = employee.InverseManagerNavigation;

            return result;
        }



        public static string GetProfileStatus(EmployeeModel employee, int currentUserId)
        {
            var displayStatus = string.Empty;

            if ((employee.Status == EmplyeeProfileStatus.NoTransaction || employee.Status == EmplyeeProfileStatus.Approved) 
                && (employee.MatrixApproval == null || Utils.MonthDifference(DateTime.Now, employee.MatrixApproval.When) >= 6))
            {
                return "OutDated";
            }
            else if (employee.MatrixApproval == null || Utils.MonthDifference(DateTime.Now, employee.MatrixApproval.When) >= 6)
            {
                displayStatus = GetProfileStatusDisplayValue(employee, currentUserId, true);
            }
            else
            {
                displayStatus = GetProfileStatusDisplayValue(employee, currentUserId, false);
            }

            return displayStatus;
        }

        private static string GetProfileStatusDisplayValue(EmployeeModel employee, int currentUserId, bool isOutDated)
        {
            var displayStatus = string.Empty;

            switch (employee.Status)
            {
                case EmplyeeProfileStatus.Open:
                    if (employee.Id == currentUserId || employee.Manager == currentUserId)
                    {
                        displayStatus = "In progress";
                    }
                    else
                    {
                        displayStatus = isOutDated ? "OutDated" : "Approved";
                    }
                    break;
                case EmplyeeProfileStatus.Submitted:
                    if (employee.Id == currentUserId || employee.Manager == currentUserId)
                    {
                        displayStatus = "Submitted";
                    }
                    else
                    {
                        displayStatus = isOutDated ? "OutDated" : "Approved";
                    }

                    break;
                case EmplyeeProfileStatus.Approved:
                    displayStatus = "Approved";
                    break;
            }

            return displayStatus;
        }
    }
}
