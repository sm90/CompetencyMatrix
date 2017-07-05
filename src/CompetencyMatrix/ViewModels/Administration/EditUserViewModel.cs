using CompetencyMatrix.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CompetencyMatrix.ViewModels.Administration
{
    public class EditUserViewModel
    {
        public EditUserViewModel()
        {
        }

        [Required]
        public string Id { get; set; }
        public string Email { get; set; }

        public string Password { get; set; }

        [DisplayName("Phone number")]
        public string PhoneNumber { get; set; }

        [DisplayName("Username")]
        public string UserName { get; set; }

        [DisplayName("Employee")]
        public int? EmployeeId { get; set; }

        [DisplayName("Role")]
        public string RoleId { get; set; }

        public static EditUserViewModel FromDbModel(Models.AspNetUsers user)
        {
            var model = new EditUserViewModel()
            {
                Id = user.Id,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                UserName = user.UserName,
                EmployeeId = user.EmployeeId
            };

            return model;
        }
    }

    public static class EditUserViewModelExtensions
    {
        public static Models.AspNetUsers ToDbModel(this EditUserViewModel model, ref Models.AspNetUsers user)
        {
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.UserName = model.UserName;
            user.EmployeeId = model.EmployeeId;

            return user;
        }
    }
}
