using Imigration.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Imigration.Web.Areas.UserPanel.Controllers
{
    public class QuestionController : BaseController
    {
        #region Ctor

        #endregion

        #region Create Question
        [Authorize]
        [HttpGet("create-question")]
        public async Task<IActionResult> CreateQuestion()
        {
            return View();
        }
        #endregion
    }
}
