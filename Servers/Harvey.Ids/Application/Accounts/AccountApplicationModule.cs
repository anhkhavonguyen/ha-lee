using Harvey.Ids.Application.Accounts.Commands.ChangePasswordCommandHandler;
using Harvey.Ids.Application.Accounts.Commands.UpdateInfoMemberAccountCommanHandler;
﻿using Harvey.Ids.Application.Accounts.Commands.ActiveCustomerCommandHandler;
using Harvey.Ids.Application.Accounts.Commands.ChangePINCommandHandler;
using Harvey.Ids.Application.Accounts.Commands.ForgotPasswordViaEmailCommandHandler;
using Harvey.Ids.Application.Accounts.Commands.ForgotPasswordViaSMSCommandHandler;
using Harvey.Ids.Application.Accounts.Commands.InitMemberAccountCommandHandler;
using Harvey.Ids.Application.Accounts.Commands.MigrationDataCommandHandler;
using Harvey.Ids.Application.Accounts.Commands.ReSendSignUpLinkCommandHandler;
using Harvey.Ids.Application.Accounts.Commands.ResetPasswordCommandHandler;
using Harvey.Ids.Application.Accounts.Commands.SetPasswordForAccountCommandHandler;
using Harvey.Ids.Application.Accounts.Commands.SignUpMemberCommandHandler;
using Harvey.Ids.Application.Accounts.Commands.UpdateCustomerProfileCommandHandler;
using Harvey.Ids.Application.Accounts.Queries.CheckPIN;
using Harvey.Ids.Application.Accounts.Queries.CheckSignUpLink;
using Harvey.Ids.Application.Accounts.Queries.GetBasicAccountInfo;
using Harvey.Ids.Application.Accounts.Queries.GetUserProfile;
using Harvey.Ids.Application.Accounts.Queries.SendPINToNumberPhoneQueryHandler;
using Microsoft.Extensions.DependencyInjection;
using Harvey.Ids.Application.Accounts.Commands.UpdateFullCustomerInfomationCommandHandler;
using Harvey.Ids.Application.Accounts.Commands.ReactiveCustomerWithNewPhoneCommandHandler;
using Harvey.Ids.Application.Accounts.Commands.ChangePhoneNumberCommandHandler;

namespace Harvey.Ids.Application.Accounts
{
    public static class AccountApplicationModule
    {
        public static void Registry(IServiceCollection services)
        {
            services.AddScoped<IGetBasicAccountInfoQueryHandler, GetBasicAccountInfoQueryHandler>();
            services.AddScoped<IChangePasswordCommandHandler, ChangePasswordCommandHandler>();
            services.AddScoped<IInitMemberAccountCommandHandler, InitMemberAccountCommandHandler>();
            services.AddScoped<ISignUpMemberCommandHandler, SignUpMemberCommandHandler>();
            services.AddScoped<IUpdateInfoMemberAccountCommanHandler, UpdateInfoMemberAccountCommanHandler>();
            services.AddScoped<IGetUserProfileQueryHandler, GetUserProfileQueryHandler>();
            services.AddScoped<IChangePINCommandHandler, ChangePINCommandHandler>();
            services.AddScoped<IResetPasswordCommandHandler, ResetPasswordCommandHandler>();
            services.AddScoped<IForgotPasswordViaEmailCommandHandler, ForgotPasswordViaEmailCommandHandler>();
            services.AddScoped<IForgotPasswordViaSMSCommandHandler, ForgotPasswordViaSMSCommandHandler>();
            services.AddScoped<ISendPINToNumberPhoneQueryHandler, SendPINToNumberPhoneQueryHandler>();
            services.AddScoped<ICheckPINQueryHandler, CheckPINQueryHandler>();
            services.AddScoped<IUpdateFullCustomerInfomationCommandHandler, UpdateFullCustomerInfomationCommandHandler>();
            services.AddScoped<IMigrationDataCommandHandler, MigrationDataCommandHandler>();
            services.AddScoped<IReSendSignUpLinkCommandHandler, ReSendSignUpLinkCommandHandler>();
            services.AddScoped<ISetPasswordForAccountCommandHandler, SetPasswordForAccountCommandHandler>();
            services.AddScoped<IUpdateCustomerProfileCommandHandler, UpdateCustomerProfileCommandHandler>();
            services.AddScoped<ICheckSignUpLink, CheckSignUpLinkHandler>();
            services.AddScoped<IActiveCustomerCommandHandler, ActiveCustomerCommandHandler>();
            services.AddScoped<IReactiveCustomerWithNewPhoneCommandHandler, ReactiveCustomerWithNewPhoneCommandHandler>();
            services.AddScoped<IChangePhoneNumberCommandHandler, ChangePhoneNumberCommandHandler>();
        }
    }
}
