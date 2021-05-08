using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project.Common;
using Project.Data;
using Project.Data.Models;
using Project.Services.Interfaces;
using Project.Services.Models.User;

namespace Project.Services.Services
{
    public class UserService : BaseService, IUserService
    {
        private readonly ProjectContext _context;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public UserService(ProjectContext context, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IMapper mapper)
            : base(context)
        {
            this._context = context;
            this._signInManager = signInManager;
            this._userManager = userManager;
            this._mapper = mapper;
        }

        public async Task<SignInResult> LoginAsync(LoginServiceModel loginServiceModel)
        {
            AppUser user = await this._context.Users
                .FirstOrDefaultAsync(x => x.UserName == loginServiceModel.Username);

            return await this._signInManager
                .PasswordSignInAsync(user, loginServiceModel.Password, loginServiceModel.RememberMe, false);
        }

        public async Task<UserServiceModel> RegisterAsync(RegisterServiceModel registerServiceModel)
        {
            await EnsureUserDoesNotExist(registerServiceModel.Username);

            AppUser newUser = this._mapper.Map<AppUser>(registerServiceModel);
            IdentityResult result = await this._userManager.CreateAsync(newUser, registerServiceModel.Password);

            if (!result.Succeeded)
            {
                await this._userManager.DeleteAsync(newUser);
                throw new InvalidOperationException(string.Format(ConstantsClass.CannotCreate, ConstantsClass.AppUser));
            }

            AppUser newlyCreatedUser = await this.QueryFullUserAsync(registerServiceModel.Username);
            await this._userManager.AddToRoleAsync(newlyCreatedUser, AppRole.User);
            await this._signInManager.PasswordSignInAsync(newlyCreatedUser, registerServiceModel.Password, registerServiceModel.RememberMe, false);

            UserServiceModel userServiceModel = this._mapper.Map<UserServiceModel>(newlyCreatedUser);
            userServiceModel.Roles = await this._userManager.GetRolesAsync(newlyCreatedUser);

            return userServiceModel;
        }

        public async Task<UserServiceModel> GetByIdAsync(Guid userId)
        {
            AppUser user = await this.QueryFullUserAsync(userId) ??
                throw new InvalidOperationException(
                    string.Format(ConstantsClass.DoesNotExist, ConstantsClass.AppUser));

            UserServiceModel userServiceModel = this._mapper.Map<UserServiceModel>(user);
            userServiceModel.Roles = await this._userManager.GetRolesAsync(user);

            return userServiceModel;
        }

        public async Task<UserServiceModel> GetLoggedInUserAsync()
        {
            AppUser loggedUser = await this._userManager.GetUserAsync(this._signInManager.Context.User);

            UserServiceModel userServiceModel = this._mapper.Map<UserServiceModel>(loggedUser);
            userServiceModel.Roles = await this._userManager.GetRolesAsync(loggedUser);

            return userServiceModel;
        }

        public async Task<UserServiceModel> EditUserAsync(UserServiceModel userServiceModel)
        {
            AppUser user = await this.QueryFullUserAsync(userServiceModel.Id);

            user.FirstName = userServiceModel.FirstName;
            user.LastName = userServiceModel.LastName;
            user.UserName = userServiceModel.Username;
            //If a user can edit his roles, add it here!

            await this._userManager.UpdateAsync(user);

            return this._mapper.Map<UserServiceModel>(user);
        }

        public async Task<UserServiceModel> RemoveUserAsync(Guid userId)
        {
            AppUser loggedUser = await this._userManager.GetUserAsync(this._signInManager.Context.User);
            AppUser userToBeDeleted = await this.QueryFullUserAsync(userId);

            string userRole = this._signInManager.Context
                .User.FindFirstValue(ClaimTypes.Role);
            bool isAdmin = userRole == AppRole.Admin; //Check here if issuer is an admin

            //Only authorize for deletion the current logged user and the admin
            if (!loggedUser.Equals(userToBeDeleted) || !isAdmin)
                throw new InvalidOperationException(ConstantsClass.Four_O_Four);

            IdentityResult result = await this._userManager.DeleteAsync(userToBeDeleted);

            if (!result.Succeeded)
                throw new InvalidOperationException(ConstantsClass.Error);

            return this._mapper.Map<UserServiceModel>(userToBeDeleted);
        }

        public Task SignOutAsync()
        {
            return this._signInManager.SignOutAsync();
        }

        public async Task<bool> PromoteToAdminAsync(Guid userId)
        {
            AppUser user = await this.QueryFullUserAsync(userId);
            IList<string> currentUserRoles = await this._userManager.GetRolesAsync(user);

            IdentityResult removedFromAllRoles = await this._userManager.RemoveFromRolesAsync(user, currentUserRoles);
            if (!removedFromAllRoles.Succeeded)
                throw new InvalidOperationException(string.Format(ConstantsClass.UnableToEdit, ConstantsClass.AppRole));

            return (await this._userManager.AddToRoleAsync(user, AppRole.Admin)).Succeeded;
        }

        /* Private Methods */

        private async Task EnsureUserDoesNotExist(Guid userId)
        {
            bool userExists = await DoesUserExist(userId);

            if (userExists)
                throw new ArgumentException(string.Format(ConstantsClass.AlreadyExists, ConstantsClass.AppUser));
        }

        private async Task EnsureUserDoesNotExist(string username)
        {
            bool userExists = await DoesUserExist(username);

            if (userExists)
                throw new ArgumentException(string.Format(ConstantsClass.AlreadyExists, ConstantsClass.AppUser));
        }

        private async Task<bool> DoesUserExist(string username)
        {
            return await this._context.Users.AnyAsync(x => x.UserName == username);
        }

        private async Task<bool> DoesUserExist(Guid userId)
        {
            return await this._context.Users.AnyAsync(x => x.Id == userId.ToString());
        }

        private async Task<AppUser> QueryFullUserAsync(Guid userId)
        {
            return await this._context.Users
                // .Include(x => x.COLLECTION)
                .FirstOrDefaultAsync(x => x.Id == userId.ToString());
        }

        private async Task<AppUser> QueryFullUserAsync(string username)
        {
            return await this._context.Users
                // .Include(x => x.COLLECTION)
                .FirstOrDefaultAsync(x => x.UserName == username);
        }
    }
}
