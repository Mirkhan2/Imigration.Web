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
        public async Task<IActionResult> LoadCreateTagPartial( )
        {

            
            return PartialView("CreateTagPartial" );
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
