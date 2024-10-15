using Imigration.Application.Services.Interfaces;
using Imigration.Domains.ViewModels.Admin.Tag;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Imigration.Web.Areas.Admin.Controllers
{
    public class HomeController : AdminBaseController
    {
        #region ctor
        private readonly IQuestionService _questionService;
        public HomeController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        #endregion

        #region Filter Tags
        public async Task<IActionResult> LoadFilterTagsPartial(FilterTagAdminViewModel filter)
        {
            filter.TakeEntity = 2;

            var result = await _questionService.FilterTagAdmin(filter);


            return PartialView("FilterTagPartial", result);
        }
        #endregion

        #region Create Tag
        public IActionResult LoadCreateTagPartial()
        {

            return PartialView("CreateTagPartial" );
        }
        [HttpPost]
        public async Task<IActionResult> CreateTag(CreateTagAdminViewModel create)
        {
            if (!ModelState.IsValid)
            {
                return new JsonResult(new { status = " error", message = "nicht guldig" });
            }
            await _questionService.CreateTagAdmin(create);

            return new JsonResult(new { status = " sucess", message = " guldig" });

        }
        #endregion

        #region EditTag
        public async Task<IActionResult> LoadEditTagPartial(long id)
        {
            var result = await _questionService.FillEditTagAdminViewModel(id);

            if (result == null)
            {
                return PartialView("_NotFoundDataPartial");
            }

            return PartialView("_EditTagPartial" , result);
        }

        [HttpPost]
        public async Task<IActionResult> EditTag(EditTagViewModel edit)
        {
            if (!ModelState.IsValid)
            {
                return new JsonResult(new { status = " error", message = "nicht guldig" });
            }

            var result = await _questionService.EditTagAdmin(edit);

            if (!result)
            {
                return new JsonResult(new { status = " error", message = "nicht guldig" });

            }

            return new JsonResult(new { status = " sucess", message = " guldig" });

        }
        #endregion

        #region DeleteTag

        [HttpPost]
        public async Task<IActionResult> DeleteTag(long id)
        {
            var result = await _questionService.DeleteTagAdmin(id);

            if (!ModelState.IsValid)
            {
                return new JsonResult(new { status = " error", message = "nicht guldig" });
            }
         

            return new JsonResult(new { status = " sucess", message = " guldig" });
        }

        #endregion

        #region online user
        public IActionResult ShowOnlineUsers()
        {
            return View();
        }
        #endregion
        #region Dashboard
        public async Task<IActionResult> Dashboard()
        {
            ViewData["ChartDataJson"] = JsonConvert.SerializeObject(await _questionService.GetTagsViewModelJson());

            return View();
        }
        #endregion
    }
}
