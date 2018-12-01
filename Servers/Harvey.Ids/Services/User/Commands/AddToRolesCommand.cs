using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.Ids.Services.User.Commands
{
    public class AddToRolesCommand
    {
        public string UserId { get; set; }
        public List<string> RoleIds { get; set; }
    }
}
