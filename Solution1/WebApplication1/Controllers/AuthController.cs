using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Context;
using WebApplication1.Helpers;
using WebApplication1.Models;
using WebApplication1.ViewModel.AuthVM;

namespace WebApplication1.Controllers
{
    public class AuthController : Controller
    {
        Day2DbContext _db { get; }
        UserManager<AppUser> _userManager { get; }
        SignInManager<AppUser> _signInManager { get;  }
        RoleManager<IdentityRole> _roleManager { get; }
        public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, Day2DbContext db, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _db = db;
            _roleManager = roleManager;
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async  Task<IActionResult> Register(RegisterVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var user = new AppUser
            {
                UserName = vm.UserName,
                Email = vm.Email,
                Fullname = vm.Fullname
            };
            var result =await _userManager.CreateAsync(user, vm.Password);
            if(!result.Succeeded)
            {
                return View(vm);
            }
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Login));
        }


        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            AppUser user;
            if (vm.EmailOrUserName.Contains("@"))
            {
                user= await _userManager.FindByEmailAsync(vm.EmailOrUserName);
            }
            else
            {
                user=await _userManager.FindByNameAsync(vm.EmailOrUserName);
            }
            if (user == null)
            {
                return View(vm);
            }
            var result =await _signInManager.CheckPasswordSignInAsync(user, vm.Password, true);
            if (!result.Succeeded)
            {
                return View(vm);
            }
            return RedirectToAction(nameof(Index), "Home");
        }

        public async Task<bool> CreateRoles()
        {
            foreach (var item in Enum.GetValues(typeof(Roles)))
            {
                if (! await _roleManager.RoleExistsAsync(item.ToString()))
                {
                    var result = await _roleManager.CreateAsync(new IdentityRole
                    {
                        Name=item.ToString(),
                    });
                    if (!result.Succeeded)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
