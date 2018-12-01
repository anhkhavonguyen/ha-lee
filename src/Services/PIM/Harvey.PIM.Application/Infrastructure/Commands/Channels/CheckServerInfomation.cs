using Harvey.Domain;
using Harvey.Persitance.EF;
using Harvey.PIM.Application.Infrastructure.Domain;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.PIM.Application.Infrastructure.Commands.Channels
{
    public class CheckServerInfomation: ICommand<bool>
    {
        public Guid Id { get; set; }
        public string ServerInformation { get; set; }
        public CheckServerInfomation(Guid id, string serverInformation)
        {
            Id = id;
            ServerInformation = serverInformation;
        }
    }

    public class CheckServerInfomationHandler : ICommandHandler<CheckServerInfomation, bool>
    {
        private readonly IEfRepository<PimDbContext, Channel> _repository;
        public CheckServerInfomationHandler(IEfRepository<PimDbContext, Channel> repository)
        {
            _repository = repository;
        }
        public async Task<bool> Handle(CheckServerInfomation command)
        {
            var result = await _repository.ListAsync(x => x.Id != command.Id && x.ServerInformation == command.ServerInformation);
            if (result.ToArray().Length == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
