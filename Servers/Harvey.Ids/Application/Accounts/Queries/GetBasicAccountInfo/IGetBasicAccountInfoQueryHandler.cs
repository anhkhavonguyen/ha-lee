using System.Threading.Tasks;

namespace Harvey.Ids.Application.Accounts.Queries.GetBasicAccountInfo
{
    public interface IGetBasicAccountInfoQueryHandler
    {
        Task<GetBasicAccountInfo> ExecuteAsync(string countryCode,string phoneNumber);
    }
}
