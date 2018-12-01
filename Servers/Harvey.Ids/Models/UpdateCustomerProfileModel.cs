using Harvey.Ids.Data;
using System;
using System.ComponentModel.DataAnnotations;

namespace Harvey.Ids.Models
{
    public class UpdateCustomerProfileModel
    {
        [Required]
        public string CustomerId { get; set; }
        [Required]
        public string CustomerCode { get; set; }
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
        [Required]
        public string StaffId { get; set; }
        [Required]
        public DateTime UpdatedDate { get; set; }
        public Gender? Gender { get; set; }
    }
}
