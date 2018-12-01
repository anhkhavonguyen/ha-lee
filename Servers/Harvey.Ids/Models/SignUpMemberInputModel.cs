using Harvey.Ids.Data;
using System;
using System.ComponentModel.DataAnnotations;

namespace Harvey.Ids.Models
{
    public class SignUpMemberInputModel
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string Password { get; set; }
        public Gender? Gender { get; set; }
        public string Avatar { get; set; }
        [Required]
        public string Code { get; set; }
    }
}
