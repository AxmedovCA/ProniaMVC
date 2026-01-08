using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Pronia.Abstraction;
using Pronia.Contexts;
using Pronia.ViewModels.UserViewModels;

namespace Pronia.Controllers
{
    public class AccountController(UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager, RoleManager<IdentityRole> _roleManager, IConfiguration configuration, IEmailService _emailService) : Controller
    {
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var existUser = await _userManager.FindByNameAsync(vm.UserName);
            if (existUser is { })
            {
                ModelState.AddModelError("UserName", "This username is already exist");
                return View(vm);
            }
            var existEmail = await _userManager.FindByEmailAsync(vm.EmailAddress);
            if (existEmail is { })
            {
                ModelState.AddModelError("EmailAddress", "This username is already exist");
                return View(vm);
            }
            AppUser user = new()
            {
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                Email = vm.EmailAddress,
                UserName = vm.UserName,

            };
            var result = await _userManager.CreateAsync(user, vm.Password);
            if (result.Succeeded == false)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(vm);
            }


            await _userManager.AddToRoleAsync(user, "Member");
            await SendConfirmationEmail(user);

            TempData["SuccessMessage"] = "Please confirm your email";

            return RedirectToAction("Login");
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
            var user = await _userManager.FindByEmailAsync(vm.EmailAddress);
            if (user is null)
            {
                ModelState.AddModelError("", "Email or password is wrong");
                return View(vm);
            }
            var result = await _userManager.CheckPasswordAsync(user, vm.Password);
            if (result == false)
            {
                ModelState.AddModelError("", "Email or password is wrong");
                return View(vm);
            }
            if (!user.EmailConfirmed)
            {
                ModelState.AddModelError("", "Please confirm your email");
                await SendConfirmationEmail(user);
                return View(vm);
            }
            await _signInManager.SignInAsync(user, vm.IsRemember);
            return RedirectToAction("Index", "Home");

        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
        public async Task<IActionResult> CreateRoles()
        {
            await _roleManager.CreateAsync(new IdentityRole()
            {
                Name = "moderator"
            });
            await _roleManager.CreateAsync(new IdentityRole()
            {
                Name = "Member"
            });
            await _roleManager.CreateAsync(new IdentityRole()
            {
                Name = "Moderator"
            });
            return Ok("Roles created");
        }
        public async Task<IActionResult> CreatemoderatorAndModerator()
        {

            var adminUserVM = configuration.GetSection("AdminUser").Get<UserVM>();
            var moderatorUserVM = configuration.GetSection("ModeratorUser").Get<UserVM>();
            if (adminUserVM is not null)
            {
                AppUser adminUser = new()
                {
                    FirstName = adminUserVM.FirstName,
                    LastName = adminUserVM.LastName,
                    Email = adminUserVM.Email,
                    UserName = adminUserVM.UserName,
                };

                await _userManager.CreateAsync(adminUser, adminUserVM.Password);
                await _userManager.AddToRoleAsync(adminUser, "Admin");
            }
            if (moderatorUserVM is not null)
            {
                AppUser moderatorUser = new()
                {
                    FirstName = moderatorUserVM.FirstName,
                    LastName = moderatorUserVM.LastName,
                    Email = moderatorUserVM.Email,
                    UserName = moderatorUserVM.UserName,
                };

                await _userManager.CreateAsync(moderatorUser, moderatorUserVM.Password);

                await _userManager.AddToRoleAsync(moderatorUser, "Moderator");
            }
            return Ok("Succesfully");
        }

        private async Task SendConfirmationEmail(AppUser user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);



            //string url = @$"https://localhost:7091/account/ConfirmEmail?token={token}&userId={user.Id}";
            string url = Url.Action("ConfirmEmail", "account", new { token = token, userId = user.Id }, Request.Scheme) ?? string.Empty;

            string emailBody = $@"<h2>Email Confirmation</h2>
                <p>Hello {user.FirstName} {user.LastName}</p>
                <p>Please confirm your email by clicking the link below:</p>
                <a href='{url}'>Confirm Email</a>";

            await _emailService.SendEmailAsync(user.Email!, "Confirm your email", emailBody);
        }
        public async Task<IActionResult> ConfirmEmail(string token, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
            {
                return BadRequest();
            }
            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (!result.Succeeded)
            {
                return BadRequest();
            }
            await _signInManager.SignInAsync(user, false);
            return RedirectToAction("Index", "Home");
        }
    }
}
