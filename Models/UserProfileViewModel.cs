using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Models
{
    public class UserProfileViewModel
    {
        public string Id {get; set;}

        [MaxLength(50, ErrorMessage = "Maximum of 50 characters")]
        [MinLength(1, ErrorMessage = "Required field")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [MaxLength(50, ErrorMessage = "Maximum of 50 characters")]
        [MinLength(1, ErrorMessage = "Required field")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [MaxLength(20, ErrorMessage = "Maximum of 20 characters")]
        [Display(Name = "Display Name")]
        public string DisplayName { get; set; }

        [Display(Name = "Avatar path")]
        public string AvatarUrl { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public HttpPostedFileBase Avatar { get; set; }

        public IEnumerable<SelectListItem> CurrentRole { get; set; }

        public IEnumerable<SelectListItem> CurrentProjects { get; set; }


    }
}