using System.Threading.Tasks;

namespace Harvey.Ids.Application.Accounts.Queries.GetUserProfile
{
    public interface IGetUserProfileQueryHandler
    {
        Task<GetUserProfileModel> ExecuteAsync(string userId);
    }
}
