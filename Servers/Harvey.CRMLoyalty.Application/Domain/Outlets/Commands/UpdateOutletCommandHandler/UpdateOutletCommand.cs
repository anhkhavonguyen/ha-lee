using Harvey.CRMLoyalty.Application.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.CRMLoyalty.Application.Domain.Outlets.Commands.UpdateOutletCommandHandler
{
    public class UpdateOutletCommand : BaseRequest
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string OutletImage { get; set; }
        public string PhoneCountryCode { get; set; }
        public string Phone { get; set; }
    }
}
