using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Harvey.Ids.Application.User.Queries.GetAllUser;
using Harvey.Ids.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Harvey.Ids.Application.User.Command.UpdateUserProfile;
using Harvey.Ids.Application.User.Command.CreateUserProfile;
using Harvey.Ids.Application.User.Queries.GetUser;
using Harvey.Ids.Application.User.Command.DeleteUserProfile;
using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Harvey.Ids.Domains;
using Microsoft.AspNetCore.Mvc.Rendering;
using Harvey.Ids.Services.Account.User;
using Harvey.Ids.Services.User.Commands;
using Newtonsoft.Json;
using Harvey.Ids.Services.Account;
using X.PagedList;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using IdentityServer4.Events;
using Harvey.Ids.Configs;
using System.Text.RegularExpressions;
using IdentityServer4.Services;

namespace Harvey.Ids.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UserController : Controller
    {
        private readonly IGetAllUserQueries _getAllUser;
        private readonly IGetUserQueries _getUserProfile;
        private readonly IUpdateUserProfileHandler _updateUserProfileHandler;
        private readonly ICreateUserProfileHandler _createUserProfileHandler;
        private readonly IDeleteUserHandler _deleteUserProfileHandler;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IUserService _userService;
        private readonly IAccountService _accountService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPersistedGrantService _persistedGrantService;

        public UserController(IGetAllUserQueries getAllUser,
                              IGetUserQueries getUserProfile,
                              IUpdateUserProfileHandler updateUserProfileHandler,
                              ICreateUserProfileHandler createUserProfileHandler,
                              IDeleteUserHandler deleteUserProfileHandler,
                              RoleManager<ApplicationRole> roleManager,
                              IUserService userService,
                              IAccountService accountService,
                              UserManager<ApplicationUser> userManager,
                              IPersistedGrantService persistedGrantService
                              )
        {
            _getAllUser = getAllUser;
            _getUserProfile = getUserProfile;
            _updateUserProfileHandler = updateUserProfileHandler;
            _createUserProfileHandler = createUserProfileHandler;
            _deleteUserProfileHandler = deleteUserProfileHandler;
            _roleManager = roleManager;
            _userService = userService;
            _accountService = accountService;
            _userManager = userManager;
            _persistedGrantService = persistedGrantService;
        }

        public IActionResult Index(int? page)
        {
            int pageSize = 20;
            int pageNumber = (page <= 0 || page == null) ? 1 : (int)page;
            var users = _getAllUser.Excecute();
            var Users = Mapper.Map<List<UserViewModel>>(users);
            var test = HttpContext.User;
            return View(Users.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.ReturnUrl = Request.Headers["Referer"].ToString();

            var systemRoles = await _userService.GetRolesAsync();
            ViewBag.RolesSelect = new SelectList(systemRoles, "Id", "Name");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserViewModel command, string ReturnUrl)
        {
            if (!ValidUserData(command))
            {
                var systemRoles = await _userService.GetRolesAsync();
                ViewBag.RolesSelect = new SelectList(systemRoles, "Id", "Name");
                return View(command);
            }

            var claim = User.Claims.FirstOrDefault(c => c.Type == "sub");
            var user = Mapper.Map<CreateUserProfile>(command);
            user.IsActive = true;
            if (claim != null)
            {
                user.RoleIds = command.SelectedRoleIds;
                var result = await _createUserProfileHandler.Execute(user, claim.Value);
                if (result == null)
                {
                    var systemRoles = await _userService.GetRolesAsync();
                    ViewBag.RolesSelect = new SelectList(systemRoles, "Id", "Name");

                    ModelState.AddModelError("", AccountOptions.InvalidEmailExists);
                    return View(command);
                }

                if (result.Item1.Succeeded)
                {
                    await _accountService.SendMailConfirmUserAsync(result.Item2, $"{Request.Scheme}://{Request.Host}");
                }
                else
                {
                    var systemRoles = await _userService.GetRolesAsync();
                    ViewBag.RolesSelect = new SelectList(systemRoles, "Id", "Name");

                    foreach (var item in result.Item1.Errors)
                    {
                        ModelState.AddModelError(item.Code, item.Description);
                    }
                    return View(command);
                }
            }
            else
            {
                return RedirectToAction("Index");
            }

            if (ReturnUrl != null)
            {
                return Redirect(ReturnUrl);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            ViewBag.ReturnUrl = Request.Headers["Referer"].ToString();
            var User = await _getUserProfile.ExecuteAsync(id);
            var user = Mapper.Map<UserViewModel>(User);

            var rolesByUser = await _userService.GetRolesAsync(id);
            user.JsonUserRoles = JsonConvert.SerializeObject(rolesByUser.Select(x => x.Id).ToList());

            var systemRoles = await _userService.GetRolesAsync();
            ViewBag.RolesSelect = new SelectList(systemRoles, "Id", "Name", user.SelectedRoleIds);
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserViewModel userEdit, string returnUrl)
        {
            if (!ValidUserData(userEdit))
            {
                var systemRoles = await _userService.GetRolesAsync();
                ViewBag.RolesSelect = new SelectList(systemRoles, "Id", "Name");
                return View(userEdit);
            }

            var claim = User.Claims.FirstOrDefault(c => c.Type == "sub");
            var user = Mapper.Map<UpdateUserProfile>(userEdit);
            var userId = await _updateUserProfileHandler.ExecuteAsync(user, claim.Value);
            if (userId == null)
            {
                var systemRoles = await _userService.GetRolesAsync();
                ViewBag.RolesSelect = new SelectList(systemRoles, "Id", "Name");

                ModelState.AddModelError("", AccountOptions.InvalidEmailExists);
                return View(userEdit);
            }

            var addToRolesCommand = new AddToRolesCommand
            {
                UserId = userId,
                RoleIds = (userEdit.SelectedRoleIds != null) ? userEdit.SelectedRoleIds : new List<string>()
            };
            await _userService.AddToRolesAsync(addToRolesCommand);

            if (!userEdit.IsActive)
            {
                await _persistedGrantService.RemoveAllGrantsAsync(userId, ClientsConfig.HarveyRimsPage);
                var userEntity = await _userManager.FindByIdAsync(userId);
                await _userManager.UpdateSecurityStampAsync(userEntity);
            }

            if (returnUrl != null)
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var loggedUserId = _userManager.GetUserId(HttpContext.User);
            if (loggedUserId == null || loggedUserId == id)
            {
                return RedirectToAction("Index");
            }

            ViewBag.ReturnUrl = Request.Headers["Referer"].ToString();
            var User = await _getUserProfile.ExecuteAsync(id);
            var user = Mapper.Map<UserViewModel>(User);
            return PartialView(user);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UserViewModel command, string ReturnUrl)
        {
            await _deleteUserProfileHandler.ExecuteAsync(command.Id);
            if (ReturnUrl != null)
            {
                return Redirect(ReturnUrl);
            }
            else
            {
                return RedirectToAction("Index");
            }

        }

        private bool ValidUserData(UserViewModel userModel)
        {
            if (userModel.SelectedRoleIds == null || !userModel.SelectedRoleIds.Any())
            {
                return false;
            }

            if (userModel.DateOfBirth == null)
            {
                return false;
            }

            if (!IsPhoneNumber(userModel.PhoneNumber))
            {
                return false;
            }

            return true;
        }

        private bool IsPhoneNumber(string number)
        {
            return Regex.Match(number, @"^[0-9]{9,12}$").Success;
        }
    }
}