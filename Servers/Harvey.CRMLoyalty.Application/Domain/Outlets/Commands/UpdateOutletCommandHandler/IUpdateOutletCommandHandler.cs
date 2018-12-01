using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Harvey.CRMLoyalty.Application.Domain.Outlets.Commands.UpdateOutletCommandHandler
{
    public interface IUpdateOutletCommandHandler
    {
        Task<string> ExecuteAsync(UpdateOutletCommand command);
    }
}
