using Harvey.Ids.Data;
using System;
using System.ComponentModel.DataAnnotations;

namespace Harvey.Ids.Models
{
    public class UpdateInfoMemberInputModel
    {
        //[Required]
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Gender? Gender { get; set; }
        public string Avatar { get; set; }
        public string ZipCode { get; set; }
        public string DateOfBirth { get; set; }
    }
}
