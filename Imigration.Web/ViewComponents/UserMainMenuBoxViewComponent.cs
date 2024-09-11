using Imigration.Application.Extensions;
using Imigration.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Imigration.Web.ViewComponents
{
    public class UserMainMenuBoxViewComponent : ViewComponent
    {

        #region Ctor

        private IUserService _userService;

        public UserMainMenuBoxViewComponent(IUserService userService)
        {
            _userService = userService;
        }

        #endregion

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userService.GetUserById(HttpContext.User.GetUserId());

            return View("UserMainMenuBox", user);
        }
    }
}
