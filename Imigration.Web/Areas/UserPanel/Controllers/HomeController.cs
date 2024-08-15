using Imigration.Application.Extensions;
using Imigration.Application.Services.Interfaces;
using Imigration.Application.Statics;
using Microsoft.AspNetCore.Mvc;

namespace Imigration.Web.Areas.UserPanel.Controllers
{
    public class HomeController : UserPanelBaseController
    {
        #region Ctor

        private IUserService _userService;
        public HomeController(IUserService userService)
        {
            _userService = userService;
        }
        #endregion

        public IActionResult Index()
        {
            return View();
        }
        #region change User Avatar
        public async Task<IActionResult> ChangeUserAvatar(IFormFile userAvatar)
        {
            var fileName = Guid.NewGuid() + Path.GetExtension(userAvatar.FileName);

            var validFormats = new List<string>()
            {
                ".prg",
                ".jpg",
                ".jpeg"
            };

            var result = userAvatar.UploadFile(fileName, PathTools.UserAvatarServerPath, validFormats);
            if (!result)
            {

                return new JsonResult(new { status = "Error" });
            }

            await _userService.ChangeUserAvatar(HttpContext.User.GetUserId(), fileName);

            TempData[SuccessMessage] = "";

            return new JsonResult(new { status = "Success" });
        }
        #endregion
    }
}
