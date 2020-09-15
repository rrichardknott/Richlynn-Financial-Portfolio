using RichlynnFinancialPortal.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RichlynnFinancialPortal.ViewModels
{
    public class UpdateProfileViewModel
    {
        public string Id { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [StringLength (50, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 50 characters.")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 50 characters.")]
        public string LastName { get; set; }

        public UpdateProfileViewModel(ApplicationUser user)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
        }

        public UpdateProfileViewModel()
        {

        }
    }
}