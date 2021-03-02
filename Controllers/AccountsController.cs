using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using webad_source.Models;
using Amazon.Extensions.CognitoAuthentication;
using Microsoft.AspNetCore.Identity;
using Amazon.AspNetCore.Identity.Cognito;
using webad_source.Models.Accounts;

namespace webad_source.Controllers
{
    public class AccountsController : Controller
    {
        private readonly ILogger<AccountsController> _logger;
        private readonly SignInManager<CognitoUser> _signInManager;
        private readonly UserManager<CognitoUser> _userManager;
        private readonly CognitoUserPool _pool;

        public AccountsController(ILogger<AccountsController> logger, SignInManager<CognitoUser> signInManager, UserManager<CognitoUser> userManager, CognitoUserPool pool)
        {
            _logger = logger;
            _userManager = userManager;
            _pool = pool;
            _signInManager = signInManager;

        }


        public IActionResult Signup()
        {
            var model = new SignupModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Signup(SignupModel model)
        {
            var user = _pool.GetUser(model.Email);

            if (ModelState.IsValid)
            {
                if(user.Status != null)
                {
                    ModelState.AddModelError("UserExists", "User with this email already exsists");
                    return View(model);
                }
            }
            user.Attributes.Add(CognitoAttribute.Name.AttributeName, model.Email);

            var createdUser = await _userManager.CreateAsync(user, model.Password);
            if (createdUser.Succeeded)
            {
                RedirectToAction("Confirm");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Confirm()
        {
            var model = new ConfirmModel();
            return View(model);

        }

        [HttpPost]
        [ActionName("Confirm")]
        public async Task<IActionResult> Confirm(ConfirmModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if( user == null)
                {
                    ModelState.AddModelError("NotFound", "A user with the email address provided was not found");
                    return View(model);
                }
                var result = await ((CognitoUserManager<CognitoUser>)_userManager).ConfirmSignUpAsync(user, model.Code, true);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");

                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError(item.Code, item.Description);
                    }

                    return View(model);
                }
            }

            return View(model);

        }

        public async Task<IActionResult> Login(LoginModel model)
        {
            return View(model);
        }

        [HttpPost]
        [ActionName("Login")]
        public async Task<IActionResult> LoginPost(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("LoginError", "Email and password do not match");

                }
            }
            return View("Login",model);
        }
    
    }
}
