﻿using System.ComponentModel.DataAnnotations;

namespace Harvey.Ids.Models
{
    public class LoginInputModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
    }
}
