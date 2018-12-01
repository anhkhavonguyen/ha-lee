using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.Message.Customers
{
    public interface UpdateGenderValueCommandMessage
    {
        List<UpdatedApplicationUser> updatedApplicationUsers { get; set; }
    }

    public class UpdatedApplicationUser
    {
        public string Id { get; set; }
        public int? Gender { get; set; }
    }
}
