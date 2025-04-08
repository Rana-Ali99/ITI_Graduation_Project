﻿using System.ComponentModel.DataAnnotations;

namespace ReadersClubDashboard.ViewModels
{
    public class LoginedUser
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
