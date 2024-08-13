using Imigration.Application.Services.Interfaces;
using Imigration.Domains.ViewModels.Account;
using Microsoft.AspNetCore.Mvc;

namespace Imigration.Web.Controllers
{
    public class AccountController : BaseController
    {
        #region Ctor
        private readonly IUserService _userService;
        public AccountController(IUserService userService)
        {
            _userService = userService;
        }
        #endregion

        #region Login
        [HttpGet("login")]
        public IActionResult Login()
        {
            return View();
        }

        #endregion

        #region Register
        [HttpGet("register")]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterViewModel register)
        {
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
                    TempData["ErrorMessage"] = "Email waredshode az qabl mojod ast";
                    break;
           
            }

            return View(result);
        }
        #endregion

    }
}
