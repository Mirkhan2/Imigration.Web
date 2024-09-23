using Imigration.Application.Services.Interfaces;
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

        #region filter Tags
        public async Task<IActionResult> LoadFilterTagsPartial()
        {
            return PartialView("FilterTagPartial");
        }
        #endregion
        public async Task<IActionResult> Dashboard()
        {
            ViewData["ChartDataJson"] = JsonConvert.SerializeObject(await _questionService.GetTagsViewModelJson()); 

            return View();
        }
    }
}
