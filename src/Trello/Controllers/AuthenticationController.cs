using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

using Trello.Models;

namespace Trello.Controllers
{
    public class AuthenticationController : Controller
    {
        private TrelloDbContext context;
        public AuthenticationController(TrelloDbContext tdb)
        {
            context = tdb;
        }

        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel());
        }

        private async Task LoginAsync(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Uri , user.Email)
            };
            
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(principal);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new UserService(context).ValidateUserCredentials(model.Username, model.Password);
                if (user != null)
                {
                    await LoginAsync(user);

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("InvalidCredentials", "Invalid credentials.");
            }

            return View(model);
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Signup()
        {
            return View(new SignupViewModel());
        }

        [HttpPost]
        public JsonResult CheckFieldAsync([FromBody] User user)
        {
            var used = true;
            if (user.Username == null)
                used = new UserService(context).CheckEmailTaken(user.Email);
            else
                used = new UserService(context).CheckUserNameTaken(user.Username);

            return Json(used);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Signup(SignupViewModel model)
        {
            if (new UserService(context).CheckEmailTaken(model.Email))
                ModelState.AddModelError(string.Empty, "Email already taken");
            if (new UserService(context).CheckUserNameTaken(model.Username))
                ModelState.AddModelError(string.Empty, "Username already taken");
            if (model.Password != model.PasswordConfirmation)
                ModelState.AddModelError(string.Empty, "Password mismatch");

            if (ModelState.IsValid)
            {
                var user = new User()
                {
                    Username = model.Username,
                    Email = model.Email,
                    Password = model.Password
                };
                user = new UserService(context).Create(user);
                if (user != null)
                {
                    await LoginAsync(user);
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("InvalidCredentials", "Invalid credentials.");
            }

            return View(model);
        }
    }
}
