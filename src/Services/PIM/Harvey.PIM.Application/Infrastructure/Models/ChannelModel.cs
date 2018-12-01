using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.PIM.Application.Infrastructure.Models
{
    public class ChannelModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ServerInformation { get; set; }
        public bool IsProvision { get; set; }
    }
}
