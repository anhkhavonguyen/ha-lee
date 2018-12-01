using Harvey.Ids.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.Ids.Application.Accounts.Commands.UpdateCustomerProfileCommandHandler
{
    public class UpdateCustomerProfileCommand
    {
        public string CustomerId { get; set; }
        public string CustomerCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Password { get; set; }
        public string StaffId { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Gender? Gender { get; set; }
    }
}
