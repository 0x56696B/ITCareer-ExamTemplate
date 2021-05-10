using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Common;
using Project.Services.Interfaces;
using Project.Services.Models.User;
using Project.Web.Models;

namespace Project.Web.Controllers
{
    [Controller]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            this._userService = userService;
            this._mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register() => View();

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterPost([FromForm] RegisterWebModel registerWebModel)
        {
            RegisterServiceModel registerServiceModel = this._mapper.Map<RegisterServiceModel>(registerWebModel);

            await this._userService.RegisterAsync(registerServiceModel);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login() => View();

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> LoginPost([FromForm] LoginWebModel loginWebModel)
        {
            LoginServiceModel loginServiceModel = this._mapper.Map<LoginServiceModel>(loginWebModel);
            Microsoft.AspNetCore.Identity.SignInResult signInResult = await this._userService.LoginAsync(loginServiceModel);

            if (!signInResult.Succeeded)
                return RedirectToAction(nameof(HomeController.Error), ConstantsClass.HomeController);

            return RedirectToAction(nameof(HomeController.Index), ConstantsClass.HomeController);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ViewProfile()
        {
            UserServiceModel userServiceModel = await this._userService.GetLoggedInUserAsync();

            if (userServiceModel is null)
                return RedirectToAction(nameof(HomeController.Index), ConstantsClass.HomeController);

            return View(this._mapper.Map<UserWebModel>(userServiceModel));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> EditProfile(Guid userId)
        {
            UserServiceModel userServiceModel = await this._userService.GetByIdAsync(userId);

            if (userServiceModel is null)
                return RedirectToAction(nameof(HomeController.Index), ConstantsClass.HomeController);

            return View(this._mapper.Map<UserWebModel>(userServiceModel));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditProfilePost(UserWebModel userWebModel)
        {
            UserServiceModel userServiceModel = this._mapper.Map<UserServiceModel>(userWebModel);

            await this._userService.EditUserAsync(userServiceModel);

            return RedirectToAction(nameof(ViewProfile));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            await this._userService.SignOutAsync();

            return RedirectToAction(nameof(HomeController.Index), ConstantsClass.HomeController);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> DeleteProfile(Guid userId)
        {
            await this._userService.SignOutAsync();
            await this._userService.RemoveUserAsync(userId);

            return RedirectToAction(nameof(HomeController.Index), ConstantsClass.HomeController);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> PromotionToAdmin(Guid userId)
        {
            await this._userService.PromoteToAdminAsync(userId);

            return RedirectToAction(nameof(ViewProfile));
        }
    }
}
