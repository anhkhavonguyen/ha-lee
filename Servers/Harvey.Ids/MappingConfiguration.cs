using AutoMapper;
using Harvey.Ids.Application.Accounts.Commands.ChangePasswordCommandHandler;
using Harvey.Ids.Application.Accounts.Commands.ChangePINCommandHandler;
using Harvey.Ids.Application.Accounts.Commands.ForgotPasswordViaEmailCommandHandler;
using Harvey.Ids.Application.Accounts.Commands.ForgotPasswordViaSMSCommandHandler;
using Harvey.Ids.Application.Accounts.Commands.InitMemberAccountCommandHandler;
using Harvey.Ids.Application.Accounts.Commands.SetPasswordForAccountCommandHandler;
using Harvey.Ids.Application.Accounts.Commands.SignUpMemberCommandHandler;
using Harvey.Ids.Application.Accounts.Commands.UpdateCustomerProfileCommandHandler;
using Harvey.Ids.Application.Accounts.Commands.UpdateFullCustomerInfomationCommandHandler;
using Harvey.Ids.Application.Accounts.Commands.UpdateInfoMemberAccountCommanHandler;
using Harvey.Ids.Application.Accounts.Queries.GetUserProfile;
using Harvey.Ids.Application.User.Command.CreateUserProfile;
using Harvey.Ids.Application.User.Command.UpdateUserProfile;
using Harvey.Ids.Application.User.Queries.GetAllUser;
using Harvey.Ids.Application.User.Queries.GetUser;
using Harvey.Ids.Domains;
using Harvey.Ids.Models;
using Harvey.Ids.ViewModels;
using Harvey.Ids.ViewModels.Accounts;

namespace Harvey.Ids
{
    public static class MappingConfiguration
    {
        public static void Execute()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<ChangePasswordInputModel, ChangePasswordCommand>();
                cfg.CreateMap<SignUpMemberInputModel, SignUpMemberCommand>();
                cfg.CreateMap<UpdateInfoMemberInputModel, UpdateInfoMemberAccountCommand>();
                cfg.CreateMap<ApplicationUser, GetUserProfileModel>();
                cfg.CreateMap<ChangePINInputModel, ChangePINCommand>();
                cfg.CreateMap<ForgotPasswordInputModel, ForgotPasswordViaEmailCommand>();
                cfg.CreateMap<ForgotPasswordInputModel, ForgotPasswordViaSMSCommand>();
                cfg.CreateMap<InitAccountMemberInputModel, InitMemberAccountCommand>();
                cfg.CreateMap<UpdateFullCustomerInfomationInputModel, UpdateFullCustomerInfomationCommand>();
                cfg.CreateMap<SetPasswordForStoreAccountInputModel, SetPasswordForAccountCommand>();
                cfg.CreateMap<UpdateCustomerProfileModel, UpdateCustomerProfileCommand>();

                cfg.CreateMap<ApplicationUser, GetAllUserModel>();
                cfg.CreateMap<GetAllUserModel, UserViewModel>();
                cfg.CreateMap<GetUserModel, UserViewModel>();
                cfg.CreateMap<CreateUserProfile, ApplicationUser>();
                cfg.CreateMap<UserViewModel, UpdateUserProfile>();
                cfg.CreateMap<UserViewModel, CreateUserProfile>();

                cfg.CreateMap<ChangePasswordViewModel, ChangePasswordCommand>();
            });
        }
    }
}
