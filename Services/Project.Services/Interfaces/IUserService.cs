using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Project.Services.Models.User;

namespace Project.Services.Interfaces
{
    public interface IUserService
    {
        Task<SignInResult> LoginAsync(LoginServiceModel loginServiceModel);

        Task<UserServiceModel> RegisterAsync(RegisterServiceModel registerServiceModel);

        Task<UserServiceModel> GetByIdAsync(Guid userId);

        Task<UserServiceModel> GetLoggedInUserAsync();

        Task<UserServiceModel> EditUserAsync(UserServiceModel userServiceModel);

        Task<UserServiceModel> RemoveUserAsync(Guid userId);

        Task SignOutAsync();

        Task<bool> PromoteToAdminAsync(Guid userId);
    }
}
