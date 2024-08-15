using System.Security.Claims;
using GoogleReCaptcha.V3.Interface;
using Imigration.Application.Services.Interfaces;
using Imigration.Domains.ViewModels.Account;
using Imigration.Web.ActionFilters;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Imigration.Web.Controllers
{
    public class AccountController : BaseController
    {
        #region Ctor
        private readonly IUserService _userService;
        ICaptchaValidator _captchaValidator;
        public AccountController(IUserService userService,ICaptchaValidator captchaValidator)
        {
            _userService = userService;
            _captchaValidator = captchaValidator;
        }
        #endregion

        #region Login
        [HttpGet("login")]
        [RedirectHomeIfLoggedInActionFilter]
        public IActionResult Login(string ReturnUrl= "")
        {
            //if (HttpContext.User.Identity.IsAuthenticated)
            //{
            //    return Redirect("/");
            //}
            var result = new LoginViewModel();
            if (!string.IsNullOrEmpty(ReturnUrl))
            {
                result.ReturnUrl = ReturnUrl;
            }
            return View(result);

        }

        [HttpPost("Login")]

        [RedirectHomeIfLoggedInActionFilter]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(login.Captcha))
            {
                TempData[ErrorMessage] = " Die Captcha-Validierung ist fehlgeschlagen. Bitte versuchen Sie es erneut";

                return View(login);
            }

            if (!ModelState.IsValid)
            {
                return View(login);
            }

            var result = await _userService.CheckUserForLogin(login);
            switch (result)
            {

                case LoginResult.UserIsBan:
                    TempData[WarningMessage] = "دسترسی شما به سایت مسدود می باشد .";
                    break;
                case LoginResult.UserNotFound:
                    TempData[ErrorMessage] = "کاربر مورد نظر یافت نشد .";
                    break;
                case LoginResult.EmailNotActivated:
                    TempData[WarningMessage] = "برای ورود به حساب کاربری ابتدا ایمیل خود را فعال کنید .";
                    break;
                case LoginResult.Success:

                    var user = await _userService.GetUserByEmail(login.Email);

                    #region Login User

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    var properties = new AuthenticationProperties { IsPersistent = login.RememberMe };

                    await HttpContext.SignInAsync(principal, properties);

                    #endregion

                    TempData[SuccessMessage] = "خوش آمدید";

                    if (!string.IsNullOrEmpty(login.ReturnUrl))
                    {
                        return Redirect(login.ReturnUrl);
                    }

                    return Redirect("/");


            }

            return View(login);
        }

        #endregion

        #region Register
        [HttpGet("register")]

        //[RedirectHomeIfLoggedInActionFilter]
     
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost("register"),ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel register)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(register.Captcha))
            {
                TempData[ErrorMessage] = " Die Captcha-Validierung ist fehlgeschlagen. Bitte versuchen Sie es erneut";
                return View(register);
                
            }
            if (!ModelState.IsValid) 
            {
                return View(register);
            }
            var result = await _userService.RegisterUser(register);

            switch (result)
            {
                case RegisterResult.Success:
                    TempData["SuccessMessage"] = " suceess ";
                    return RedirectToAction("Login", "Account");
         
                case RegisterResult.EmailExists:
                    TempData["ErrorMessage"] = "Die Operation war erfolgreich";
                    break;
           
            }

            return View(result);
        }
        #endregion

        #region Logout
        [HttpGet("Logout")]

        [RedirectHomeIfLoggedInActionFilter]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }
        #endregion

        #region Email Activation
        [HttpGet("Acticate-Email/{activationCode}")]
        [RedirectHomeIfLoggedInActionFilter]
        public async Task<IActionResult> ActivationUserEmail(string activationCode)
        {
            var result = await _userService.ActivateUserEmail(activationCode);

            if (result)
            {
                TempData[SuccessMessage] = "Hesab ba moafaqiat anjam shod";
            }
            else
            {
                TempData[ErrorMessage] = "";
            }
            return RedirectToAction("Login", "Account");
        }
        #endregion

        #region Forgot Password
        [HttpGet("Forgot-Password")]
        public async Task<IActionResult> ForgotPAssword()
        {
            return View();
        }
        [HttpPost("Forgot-Password"), ValidateAntiForgeryToken]
        [RedirectHomeIfLoggedInActionFilter]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel forgot)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(forgot.Captcha))
            {
                TempData[ErrorMessage] = "اعتبار سنجی Captcha با خطا مواجه شد لطفا مجدد تلاش کنید .";
                return View(forgot);
            }

            if (!ModelState.IsValid)
            {
                return View(forgot);
            }

            var result = await _userService.ForgotPassword(forgot);

            switch (result)
            {
                case ForgotPasswordResult.UserBan:
                    TempData[WarningMessage] = "دسترسی شما به حساب کاربری مسدود می باشد .";
                    break;
                case ForgotPasswordResult.UserNotFound:
                    TempData[ErrorMessage] = "کاربری با مشخصات وارد شده یافت نشد .";
                    break;
                case ForgotPasswordResult.Success:
                    TempData[InfoMessage] = "لینک بازیابی رمز عبور به ایمیل شما ارسال شد .";
                    return RedirectToAction("Login", "Account");
            }

            return View(forgot);
        }

        #endregion

        #region ResetPassword

   
        [HttpGet("Reset-Password/{emailActivationCode}")]
        public async Task<IActionResult> ResetPassword(string emailActivationCode)
        {
            var user = await _userService.GetUserByActivationCode(emailActivationCode);

            if (user == null || user.IsBan || user.IsDelete) return NotFound();

            return View(new ResetPasswordViewModel
            {
                EmailActivationCode = user.EmailActivationCode
            });
        }

        [HttpPost("Reset-Password/{emailActivationCode}"), ValidateAntiForgeryToken]
        [RedirectHomeIfLoggedInActionFilter]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel reset)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(reset.Captcha))
            {
                TempData[ErrorMessage] = "اعتبار سنجی Captcha با خطا مواجه شد لطفا مجدد تلاش کنید .";
                return View(reset);
            }

            if (!ModelState.IsValid)
            {
                return View(reset);
            }

            var result = await _userService.ResetPassword(reset);

            switch (result)
            {
                case ResetPasswordResult.Success:
                    TempData[SuccessMessage] = "عملیات با موفقیت انجام شد .";
                    return RedirectToAction("Login", "Account");
                case ResetPasswordResult.UserNotFound:
                    TempData[ErrorMessage] = "کاربر مورد نظر یافت نشد .";
                    break;
                case ResetPasswordResult.UserIsBan:
                    TempData[WarningMessage] = "دسترسی شما به سایت مسدود می باشد .";
                    break;
            }

            return View(reset);
        }

        #endregion
    }
}
