using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Trello.Models
{
    public class SignupViewModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string PasswordConfirmation { get; set; }
    }
}