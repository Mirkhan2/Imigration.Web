using Imigration.Application.Extensions;
using Imigration.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Imigration.Web.Areas.UserPanel.ViewComponents
{
    public class UserSidebarViewComponent : ViewComponent
    {
        #region Ctor
        private  IUserService _userService;
        public UserSidebarViewComponent(IUserService userService)
        {
            _userService = userService;
        }
        #endregion

        public async Task<IViewComponentResult> InvokeAsync()
        {
           var user = await _userService.GetUserById(HttpContext.User.GetUserId());
            

            return View("UserSidebar" , user);
        }
    }
}
