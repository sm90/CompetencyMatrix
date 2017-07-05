﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApplicationCore.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Employees Name")]
        public int EmployeeId { get; set; }

        public List<SelectListItem> Employees { get; set; }

        public ComplexEmployee Complex { get; set; }

        public ComplexRole ComplexRoles { get; set; }

        public string RoleId { get; set; }

        public List<SelectListItem> Roles { get; set; }
    }

    public class ComplexEmployee
    {
        public List<SelectListItem> Employees { get; set; }

        [Required]
        [Display(Name = "Employees Name")]
        public int EmployeeId { get; set; }
    }

    public class ComplexRole
    {
        public List<SelectListItem> Roles { get; set; }

        [Required]
        [Display(Name = "Role Name")]
        public string RoleId { get; set; }
    }
}