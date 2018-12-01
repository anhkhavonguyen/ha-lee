using AutoMapper;
using Harvey.Ids.Application.Accounts.Commands.ChangePasswordCommandHandler;
using Harvey.Ids.Application.Accounts.Commands.UpdateInfoMemberAccountCommanHandler;
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
using Harvey.Ids.Configs;
using Harvey.Ids.Domains;
using Harvey.Ids.Models;
using Harvey.Ids.Services.ClientContextService;
using Harvey.Ids.Utils;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Harvey.Ids.Application.Accounts.Commands.UpdateFullCustomerInfomationCommandHandler;
using Microsoft.EntityFrameworkCore;
using Harvey.Ids.Application.Accounts.Commands.ChangePhoneNumberCommandHandler;

namespace Harvey.Ids.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Account")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public partial class AccountController : Controller
    {
        private readonly IGetBasicAccountInfoQueryHandler _getBasicAccountInfo;
        private readonly IChangePasswordCommandHandler _changePasswordCommandHandler;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IClientContextService _clientContextService;
        private readonly IInitMemberAccountCommandHandler _initMemberAccountCommandHandler;
        private readonly ISignUpMemberCommandHandler _signUpMemberCommandHandler;
        private readonly IUpdateInfoMemberAccountCommanHandler _updateInfoMemberAccountCommanHandler;
        private readonly IGetUserProfileQueryHandler _getUserProfile;
        private readonly IChangePINCommandHandler _changePINCommandHandler;
        private readonly IResetPasswordCommandHandler _resetPasswordCommandHandler;
        private readonly IForgotPasswordViaEmailCommandHandler _forgotPasswordViaEmailCommandHandler;
        private readonly ISendPINToNumberPhoneQueryHandler _sendPINToNumberPhoneQueryHandler;
        private readonly ICheckPINQueryHandler _checkPINQueryHandler;
        private readonly IUpdateFullCustomerInfomationCommandHandler _updateFullCustomerInfomationCommandHandler;
        private readonly IMigrationDataCommandHandler _migrationDataCommandHandler;
        private readonly IReSendSignUpLinkCommandHandler _reSendSignUpLinkCommandHandler;
        private readonly ISetPasswordForAccountCommandHandler _setPasswordForAccountCommandHandler;
        private readonly IForgotPasswordViaSMSCommandHandler _forgotPasswordViaSMSCommandHandler;
        private readonly IUpdateCustomerProfileCommandHandler _updateCustomerProfileCommandHandler;
        private readonly IChangePhoneNumberCommandHandler _changePhoneCommandHandler;
        private readonly ICheckSignUpLink _checkSignUpLink;
        private IOptions<ConfigurationRabbitMq> _configurationRabbitMq;

        public AccountController(IGetBasicAccountInfoQueryHandler getBasicAccountInfo,
            IChangePasswordCommandHandler changePasswordCommandHandler,
            UserManager<ApplicationUser> userManager,
            IClientContextService clientContextService,
            IInitMemberAccountCommandHandler initMemberAccountCommandHandler,
            ISignUpMemberCommandHandler signUpMemberCommandHandler,
            IUpdateInfoMemberAccountCommanHandler updateInfoMemberAccountCommanHandler,
            IGetUserProfileQueryHandler getUserProfile,
            IChangePINCommandHandler changePINCommandHandler,
            IForgotPasswordViaEmailCommandHandler forgotPasswordCommandHandler,
            IResetPasswordCommandHandler resetPasswordCommandHandler,
            ISendPINToNumberPhoneQueryHandler sendPINToNumberPhoneQueryHandler,
            ICheckPINQueryHandler checkPINQueryHandler,
            IUpdateFullCustomerInfomationCommandHandler updateFullCustomerInfomationCommandHandler,
            IMigrationDataCommandHandler migrationDataCommandHandler,
            IReSendSignUpLinkCommandHandler reSendSignUpLinkCommandHandler,
            ISetPasswordForAccountCommandHandler setPasswordForAccountCommandHandler,
            IForgotPasswordViaSMSCommandHandler forgotPasswordViaSMSCommandHandler,
            IUpdateCustomerProfileCommandHandler updateCustomerProfileCommandHandler,
            IChangePhoneNumberCommandHandler changePhoneNumberCommandHandler,
            ICheckSignUpLink checkSignUpLink,
            IOptions<ConfigurationRabbitMq> configurationRabbitMq)
        {
            _getBasicAccountInfo = getBasicAccountInfo;
            _changePasswordCommandHandler = changePasswordCommandHandler;
            _userManager = userManager;
            _clientContextService = clientContextService;
            _initMemberAccountCommandHandler = initMemberAccountCommandHandler;
            _signUpMemberCommandHandler = signUpMemberCommandHandler;
            _updateInfoMemberAccountCommanHandler = updateInfoMemberAccountCommanHandler;
            _getUserProfile = getUserProfile;
            _changePINCommandHandler = changePINCommandHandler;
            _forgotPasswordViaEmailCommandHandler = forgotPasswordCommandHandler;
            _resetPasswordCommandHandler = resetPasswordCommandHandler;
            _sendPINToNumberPhoneQueryHandler = sendPINToNumberPhoneQueryHandler;
            _checkPINQueryHandler = checkPINQueryHandler;
            _updateFullCustomerInfomationCommandHandler = updateFullCustomerInfomationCommandHandler;
            _migrationDataCommandHandler = migrationDataCommandHandler;
            _reSendSignUpLinkCommandHandler = reSendSignUpLinkCommandHandler;
            _setPasswordForAccountCommandHandler = setPasswordForAccountCommandHandler;
            _forgotPasswordViaSMSCommandHandler = forgotPasswordViaSMSCommandHandler;
            _updateCustomerProfileCommandHandler = updateCustomerProfileCommandHandler;
            _changePhoneCommandHandler = changePhoneNumberCommandHandler;
            _checkSignUpLink = checkSignUpLink;
            _configurationRabbitMq = configurationRabbitMq;
        }

        [HttpGet]
        [Route("GetBasicAccountInfo")]
        [AllowAnonymous]
        public async Task<IActionResult> GetBasicAccountInfo(string countryCode, string phoneNumber)
        {
            return Ok(await _getBasicAccountInfo.ExecuteAsync(countryCode, phoneNumber));
        }

        [HttpGet]
        [Route("GetUserProfile")]
        public async Task<IActionResult> GetUserProfile()
        {
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "sub");
            if (idClaim == null) throw new InvalidOperationException("Current user not found");
            return Ok(await _getUserProfile.ExecuteAsync(idClaim.Value));
        }

        [HttpPut]
        [Route("ChangePassword")]
        [Authorize(Roles = "AdminStaff,Member,Administrator")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordInputModel changePasswordInputModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "sub");
            if (idClaim == null) throw new InvalidOperationException("Current user not found");

            await _changePasswordCommandHandler.ExecuteAsync(idClaim.Value, Mapper.Map<ChangePasswordCommand>(changePasswordInputModel));
            return Ok(ModelState);
        }

        [HttpPost]
        [Route("ForgotPasswordViaEmail")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPasswordViaEmail([FromBody] ForgotPasswordInputModel forgotPasswordInputModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == forgotPasswordInputModel.UserName && x.IsActive);
                if (user == null)
                {
                    return BadRequest();
                }
                var model = Mapper.Map<ForgotPasswordViaEmailCommand>(forgotPasswordInputModel);
                model.User = user;
                await _forgotPasswordViaEmailCommandHandler.ExecuteAsync(model);
                return Ok();
            }
            return BadRequest(ModelState);
        }

        [HttpPost]
        [Route("ForgotPasswordViaSMS")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPasswordViaSMS([FromBody] ForgotPasswordInputModel forgotPasswordInputModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == forgotPasswordInputModel.UserName && x.IsActive);
                if (user == null)
                {
                    return BadRequest();
                }
                var model = Mapper.Map<ForgotPasswordViaSMSCommand>(forgotPasswordInputModel);
                model.User = user;
                await _forgotPasswordViaSMSCommandHandler.ExecuteAsync(model);
                return Ok();
            }
            return BadRequest(ModelState);
        }

        [HttpPost]
        [Route("ResetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordInputModel resetPasswordInputModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _resetPasswordCommandHandler.ExecuteAsync(resetPasswordInputModel.Code, resetPasswordInputModel.Password);
            return Ok();
        }
        /// <summary>
        /// Update customer profile at store app
        /// </summary>
        /// <param name="updateCustomerProfileModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateCustomerProfile")]
        [Authorize(Roles = "AdminStaff,Staff")]
        public async Task<IActionResult> UpdateCustomerProfile([FromBody] UpdateCustomerProfileModel updateCustomerProfileModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await _updateCustomerProfileCommandHandler.ExecuteAsync(Mapper.Map<UpdateCustomerProfileCommand>(updateCustomerProfileModel));

            return Ok();
        }

        /// <summary>
        /// Update customer infomation at admin app
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateFullCustomerInfomation")]
        [Authorize(Roles = "Administrator,AdminStaff")]
        public async Task<IActionResult> UpdateFullCustomerInfomation([FromBody]UpdateFullCustomerInfomationCommand command)
        {
            command.UserId = User.GetUserId();
            var result = await _updateFullCustomerInfomationCommandHandler.ExecuteAsync(command);
            return Ok(result);
        }

        /// <summary>
        /// Update customer profile at member app
        /// </summary>
        /// <param name="registerMemberInputModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateProfile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateInfoMemberInputModel registerMemberInputModel)
        {
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "sub");
            registerMemberInputModel.Id = idClaim.Value;

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var updateInfoMemberAccountCommand = Mapper.Map<UpdateInfoMemberAccountCommand>(registerMemberInputModel);
            updateInfoMemberAccountCommand.CurrentUserId = User.GetUserId();
            await _updateInfoMemberAccountCommanHandler.ExecuteAsync(updateInfoMemberAccountCommand);
            return Ok();
        }

        [HttpPost]
        [Route("SignUp")]
        [AllowAnonymous]
        public async Task<IActionResult> SignUp([FromBody] SignUpMemberInputModel signUpMemberInputModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await _signUpMemberCommandHandler.ExecuteAsync(Mapper.Map<SignUpMemberCommand>(signUpMemberInputModel));
            return Ok();
        }

        [HttpPost]
        [Route("ChangePIN")]
        public async Task<IActionResult> ChangePIN([FromBody] ChangePINInputModel changePINInputModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var changePINCommand = Mapper.Map<ChangePINCommand>(changePINInputModel);
            changePINCommand.UserId = User.GetUserId();
            await _changePINCommandHandler.ExecuteAsync(changePINCommand);
            return Ok();
        }


        [HttpGet]
        [Route("SendPINToNumberPhone")]
        public async Task<IActionResult> SendPINToNumberPhone([FromQuery]SendPINToNumberPhone sendPINToNumberPhone)
        {
            sendPINToNumberPhone.OutletName = WebUtility.UrlDecode(sendPINToNumberPhone.OutletName);
            sendPINToNumberPhone.AcronymBrandName = WebUtility.UrlDecode(sendPINToNumberPhone.AcronymBrandName);
            await _sendPINToNumberPhoneQueryHandler.ExecuteAsync(sendPINToNumberPhone.CountryCode, sendPINToNumberPhone.NumberPhone, sendPINToNumberPhone.AcronymBrandName, sendPINToNumberPhone.OutletName, User.GetUserId());
            return Ok();
        }

        [HttpGet]
        [Route("CheckPIN")]
        [AllowAnonymous]
        public async Task<IActionResult> CheckPIN(string userId, string PIN)
        {
            return Ok(await _checkPINQueryHandler.ExecuteAsync(userId, PIN));
        }

        [HttpGet]
        [Route("GetRolesOfCurrentUser")]
        public IActionResult GetRolesOfCurrentUser()
        {
            var roles = ((ClaimsIdentity)User.Identity).Claims
                .Where(c => c.Type == JwtClaimTypes.Role)
                .Select(c => c.Value);
            return Ok(new
            {
                Roles = roles
            });
        }

        [HttpPost("uploadfile")]
        [Authorize(Roles = "Administrator")]
        public IActionResult UploadFile()
        {
            string message = "";
            try
            {
                var file = Request.Form.Files[0];
                if (file.Length > 0)
                {
                    var listCommand = new List<MigrationDataCommand>();
                    using (var streamReader = new StreamReader(file.OpenReadStream()))
                    {
                        string line = "";
                        while ((line = streamReader.ReadLine()) != null)
                        {
                            var data = line.Split(new[] { ',' });
                            var model = new MigrationDataCommand()
                            {
                                CustomerId = data[0],
                                FirstName = data[1],
                                LastName = data[2],
                                Email = data[3],
                                FullPhoneNumber = data[4],
                                JoinedDate = data[5],
                                LastUsedDate = data[6],
                                Status = data[7],
                                DateOfBirth = data[8],
                            };
                            listCommand.Add(model);
                        }
                    }
                    listCommand.RemoveAt(0);
                    message = _migrationDataCommandHandler.Execute(listCommand);
                }
                return Ok(message);
            }
            catch (Exception ex)
            {
                return Json("Upload Failed: " + ex.Message);
            }

        }

        [HttpGet]
        [Route("GetTime")]
        [AllowAnonymous]
        public IActionResult GetTime()
        {
            return Ok(DateTime.UtcNow);
        }

        [HttpPost]
        [Route("ReSendSignUpLink")]
        [Authorize(Roles = "Staff,AdminStaff,Member")]
        public async Task<IActionResult> ReSendSignUpLink([FromBody]ReSendSignUpLinkCommandRequest ReSendSignUpLinkCommand)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await _reSendSignUpLinkCommandHandler.ExecuteAsync(ReSendSignUpLinkCommand);
            return Ok();
        }

        [HttpPost]
        [Route("SetPasswordForStoreAccount")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> SetPasswordForStoreAccount([FromBody]SetPasswordForStoreAccountInputModel setPasswordForStoreAccountInputModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await _setPasswordForAccountCommandHandler.ExecuteAsync(Mapper.Map<SetPasswordForAccountCommand>(setPasswordForStoreAccountInputModel));
            return Ok();
        }

        [HttpGet]
        [Route("CheckSignUpLink")]
        [AllowAnonymous]
        public async Task<IActionResult> CheckSignUpLink(string code)
        {
            return Ok(await _checkSignUpLink.ExecuteAsync(code));
        }

        [HttpPost("updateGenderValue")]
        [Authorize(Roles = "Administrator,AdminStaff")]
        public async Task<IActionResult> UpdateGenderValueAsync()
        {
            return Ok(await _migrationDataCommandHandler.ExecuteAsync());
        }

        [HttpPost]
        [Route("ChangePhoneNumber")]
        [Authorize(Roles = "AdminStaff,Staff")]
        public async Task<IActionResult> ChangePhoneNumber([FromBody] ChangePhoneNumberModel changePhoneNumberModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await _changePhoneCommandHandler.ExecuteAsync(Mapper.Map<ChangePhoneNumberCommand>(changePhoneNumberModel));

            return Ok(result);
        }
    }
}