using AutoMapper;
using ESourcing.Core.Entities;
using ESourcing.UI.Extension;
using ESourcing.UI.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ESourcing.UI.Controllers
{
    public class HomeController : Controller
    {

        readonly IMapper _mapper;
        UserManager<AppUser> _userManager { get; }
        SignInManager<AppUser> _signManager { get; }

        public HomeController(IMapper mapper, UserManager<AppUser> userManager, SignInManager<AppUser> signManager)
        {
            _mapper = mapper;
            _userManager = userManager;
            _signManager = signManager;
        }


        [Authorize]
        public IActionResult Index()
        {
            return View();
        }


        [NotSession]

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [NotSession]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginModel)
        {


            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(loginModel.Email);

                if (user != null)
                {
                    await _signManager.SignOutAsync();

                    var result = await _signManager.PasswordSignInAsync(user, loginModel.Password,true, false);

                    if (result.Succeeded)
                    {
                        HttpContext.Session.SetString("IsAdmin", user.isAdmin.ToString());
                        return RedirectToAction("Index");

                    }
                }
                ModelState.AddModelError("", "Email adress is not valid or password");
            }

            return View();

        }

        [NotSession]
        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        [NotSession]

        public async Task<IActionResult> Signup(AppUserViewModel signupModel)
        {
            
            if (ModelState.IsValid)
            {
                var user = signupModel.getUser(_mapper); 

                var result = await _userManager.CreateAsync(user, signupModel.Password);

                if (result.Succeeded)
                    return RedirectToAction("Login");
                else
                {
                    foreach (IdentityError item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }
            return View(signupModel);
        }

        [Authorize]
        public IActionResult Logout()
        {
            _signManager.SignOutAsync();
            HttpContext.Session.Remove("IsAdmin");
            return RedirectToAction("Login");
        }


        #region Social Media Login

        public IActionResult FacebookLogin(string returnUrl)
        {
            string redirectUrl = Url.Action("SocialMediaReponse", "Home", new { returnUrl });
            var properties = _signManager.ConfigureExternalAuthenticationProperties("Facebook", redirectUrl);
            return new ChallengeResult("Facebook", properties);
        }


        public IActionResult GoogleLogin(string returnUrl)
        {
            string redirectUrl = Url.Action("SocialMediaReponse", "Home", new { returnUrl });
            var properties = _signManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
            return new ChallengeResult("Google",properties);
        }
        public async Task<IActionResult> SocialMediaReponse(string returnUrl)
        {
            var loginInfo =await _signManager.GetExternalLoginInfoAsync();
            if(loginInfo == null)
                return RedirectToAction("Signup");
            else
            {
                var result = await _signManager.ExternalLoginSignInAsync(loginInfo.LoginProvider, loginInfo.ProviderKey, true);
                if(result.Succeeded)
                {
                    if(returnUrl!=null)
                    return Redirect(returnUrl);
                }
                else
                {
                    if (loginInfo.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
                    {
                        var b = loginInfo.Principal;
                        var c = loginInfo.Principal.Claims;
                        var d = loginInfo.Principal.FindFirstValue(ClaimTypes.Name);

                        AppUser user = new AppUser()
                        {
                            Email = loginInfo.Principal.FindFirstValue(ClaimTypes.Email),
                            UserName = loginInfo.Principal.FindFirstValue(ClaimTypes.Name),
                        };

                        var createResult = await _userManager.CreateAsync(user);
                        if(createResult.Succeeded)
                        {
                            var identityLogin = await _userManager.AddLoginAsync(user, loginInfo);
                            if(identityLogin.Succeeded)
                            {
                                await _signManager.SignInAsync(user, true);
                                if (returnUrl != null)
                                    return Redirect(returnUrl);


                            }
                        }
                    }

                }
            }

           return RedirectToAction("Index","Home");
        }

        #endregion
    }
}
