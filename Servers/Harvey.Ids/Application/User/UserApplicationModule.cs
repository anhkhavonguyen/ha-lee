using Harvey.Ids.Application.User.Command.CreateUserProfile;
using Harvey.Ids.Application.User.Command.DeleteUserProfile;
using Harvey.Ids.Application.User.Command.UpdateUserProfile;
using Harvey.Ids.Application.User.Queries.GetAllUser;
using Harvey.Ids.Application.User.Queries.GetUser;
using Microsoft.Extensions.DependencyInjection;

namespace Harvey.Ids.Application.User
{
    public static class UserApplicationModule
    {
        public static void Register(IServiceCollection services)
        {
            services.AddScoped<IGetAllUserQueries, GetAllUserQueries>();  
            services.AddScoped<ICreateUserProfileHandler, CreateUserProfileHandler>();
            services.AddScoped<IUpdateUserProfileHandler, UpdateUserProfileHandler>();
            services.AddScoped<IGetUserQueries, GetUserQueries>();
            services.AddScoped<IDeleteUserHandler, DeleteUserHandler>();
        }
    }
}
