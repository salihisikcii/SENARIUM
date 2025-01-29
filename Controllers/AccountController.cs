using BlogApps.Entity;
using BlogApps.Models;
using BlogApps.UserModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogApps.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<User> _userManager;
        private RoleManager<UserRole> _roleManager;
        private SignInManager<User> _signInManager;
        // private IEmailSender _emailSender;
        public AccountController(
            UserManager<User> userManager,
            RoleManager<UserRole> roleManager,
            SignInManager<User> signInManager
            //, IEmailSender emailSender
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            // _emailSender = emailSender;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    await _signInManager.SignOutAsync();

                    // if(!await _userManager.IsEmailConfirmedAsync(user))
                    // {
                    //     ModelState.AddModelError("", "Hesabınızı onaylayınız.");
                    //     return View(model);
                    // }

                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, true);

                    if (result.Succeeded)
                    {
                        await _userManager.ResetAccessFailedCountAsync(user);
                        await _userManager.SetLockoutEndDateAsync(user, null);

                        return RedirectToAction("Index", "Home");
                    }
                    else if (result.IsLockedOut)
                    {
                        var lockoutDate = await _userManager.GetLockoutEndDateAsync(user);
                        var timeLeft = lockoutDate.Value - DateTime.UtcNow;
                        ModelState.AddModelError("", $"Hesabınız kitlendi, Lütfen {timeLeft.Minutes} dakika sonra deneyiniz");
                    }
                    else
                    {
                        ModelState.AddModelError("", "-Parolanız hatalı");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "-Bu email adresiyle bir hesap bulunamadı");
                }
            }
            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    FullName = model.FullName,
                };

                IdentityResult result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Üye");

                    // var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    // var url = Url.Action("ConfirmEmail","Account", new { user.Id, token} );

                    // // email
                    // await _emailSender.SendEmailAsync(user.Email, "Hesap Onayı", $"Lütfen email hesabınızı onaylamak için linke <a href='http://localhost:5297{url}'>tıklayınız.</a>");

                    // TempData["message"]  = "Email hesabınızdaki onay mailini tıklayınız."; 
                    return RedirectToAction("Login", "Account");
                }

                foreach (IdentityError err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
            }
            return View(model);
        }

        // public async Task<IActionResult> ConfirmEmail(string Id, string token)
        // {
        //     if(Id == null || token == null)
        //     {
        //         TempData["message"]  = "Geçersiz token bilgisi";
        //         return View();
        //     }

        //     var user = await _userManager.FindByIdAsync(Id);

        //     if(user != null)
        //     {
        //         var result = await _userManager.ConfirmEmailAsync(user, token);

        //         if(result.Succeeded)
        //         {
        //             TempData["message"]  = "Hesabınız onaylandı";
        //             return RedirectToAction("Login","Account");
        //         }
        //     }

        //     TempData["message"]  = "Kullanıcı bulunamadı";
        //     return View();
        // }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        // public IActionResult ForgotPassword()
        // {
        //    return View();
        // }

        // [HttpPost]
        // public async Task<IActionResult> ForgotPassword(string Email)
        // {
        //     if(string.IsNullOrEmpty(Email))
        //     {
        //         TempData["message"] = "Eposta adresinizi giriniz.";
        //         return View();
        //     }

        //     var user = await _userManager.FindByEmailAsync(Email);

        //     if(user == null)
        //     {
        //         TempData["message"] = "Eposta adresiyle eşleşen bir kayıt yok.";
        //         return View();
        //     }

        //     var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        //     var url = Url.Action("ResetPassword","Account", new { user.Id, token} );

        //     await _emailSender.SendEmailAsync(Email, "Parola Sıfırlama", $"Parolanızı yenilemek için linke <a href='http://localhost:5034{url}'>tıklayınız.</a>.");

        //     TempData["message"] = "Eposta adresinize gönderilen link ile şifrenizi sıfırlayabilirsiniz.";

        //     return View();

        // }

        public IActionResult ResetPassword(string Id, string token)
        {
            if (Id == null || token == null)
            {
                return RedirectToAction("Login");
            }

            var model = new ResetPasswordModel { Token = token };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    TempData["message"] = "Bu mail adresiyle eşleşen kullanıcı yok.";
                    return RedirectToAction("Login");
                }
                var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);

                if (result.Succeeded)
                {
                    TempData["message"] = "Şifreniz değiştirildi";
                    return RedirectToAction("Login");
                }

                foreach (IdentityError err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
            }
            return View(model);
        }

    }
}