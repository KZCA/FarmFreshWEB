using FarmFreshWEB.Models;
using FarmFreshWEB.Services;
using Microsoft.AspNetCore.Mvc;

namespace FarmFreshWEB.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index() => View();//(_userManager.Users.tolist());

        public IActionResult Create()=>View();

        [HttpPost]
        public async Task<IActionResult> Create(UserModel user)
        {
            if(ModelState.IsValid)
            {
                if(await _userService.CreateUser(user))
                {
                    return Redirect("/admin/product");
                }

                ModelState.AddModelError("", "Something went wrong!");
            }

            return View(user);
        }
       
        public IActionResult Login(string returnUrl) => View(new LoginViewModel { ReturnUrl = returnUrl});

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            if (ModelState.IsValid)
            {
                if(await _userService.Login(loginVM))
                {
                    return Redirect(loginVM.ReturnUrl ?? "/");
                }
				ModelState.AddModelError("", "Invalid username or password");
			}

            return View(loginVM);
           
        }

        public async Task<RedirectResult> Logout(string returnUrl = "/")
        {
            await _userService.Logout();

            return Redirect(returnUrl);
        }
    }
}
