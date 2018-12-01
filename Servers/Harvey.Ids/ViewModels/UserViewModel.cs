using Harvey.Ids.Data;
using Harvey.Ids.Domains;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.Ids.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Date)]
        [Required]
        public DateTime DateOfBirth { get; set; }
        public Gender? Gender { get; set; }
        public UserType UserType { get; set; }

        [Required(ErrorMessage = "You must provide a phone number")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^[0-9]{9,12}$", ErrorMessage = "Not a valid phone number")]
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        [Required(ErrorMessage = "Error: Must Choose a Role")]
        public List<string> SelectedRoleIds { get; set; }
        public string JsonUserRoles { get; set; }
    }
}
