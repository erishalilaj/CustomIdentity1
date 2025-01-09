using AspNetCoreGeneratedDocument;
using CustomIdentity.Models;
using CustomIdentity.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace CustomIdentity.Controllers
{
    public class AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager) : Controller
    {

        private readonly SignInManager<AppUser> _signInManager = signInManager;
        private readonly UserManager<AppUser> _userManager = userManager;

        [HttpGet]
        public async Task<IActionResult> Login(string? returnUrl = null)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user != null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(model.Username!, model.Password!, model.RememberMe, false);
            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Invalid login attempt!");
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new()
                {
                    Name = model.Name,
                    UserName = model.Email,
                    Email = model.Email,
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Employees", "Account");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Employees()
        {
            List<AppUser> users = await _userManager.Users.ToListAsync();
            return View(users);
        }

            //if (ModelState.IsValid)
            //{
            //    //LOGIN
            //    var result = await signInManager.PasswordSignInAsync(model.Username!, model.Password!, model.RememberMe, false);
            //    if (result.Succeeded)
            //    {
            //        return RedirectToAction("Index", "Home");

            //    }
            //    ModelState.AddModelError("", "Invalid login attemp!");
            //    return View(model);
            //}
            //return View(model);
      



        //        public IActionResult Register()
        //        {
        //            return View();
        //        }

        //        [HttpPost]
        //        public async Task<IActionResult> Register(RegisterVM model)
        //        {
        //            if (ModelState.IsValid)
        //            {
        //                AppUser user = new()
        //                {
        //                    Name = model.Name,
        //                    UserName = model.Email,
        //                    Email = model.Email,
        //                    Address = model.Address,
        //                };
        //                var result = await userManager.CreateAsync(user, model.Password);

        //                if (result.Succeeded)
        //                {
        //                    await signInManager.SignInAsync(user,false);

        //                    return RedirectToAction("Index", "Home");
        //                }
        //                foreach(var error in result.Errors)
        //                {
        //                    ModelState.AddModelError("", error.Description);
        //                }
        //            }
        //            return View(model);
        //        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
