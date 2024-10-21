using System.Diagnostics;
using Imigration.Application.Extensions;
using Imigration.Application.Services.Interfaces;
using Imigration.Application.Statics;
using Imigration.Domains.ViewModels.Question;
using Microsoft.AspNetCore.Mvc;

namespace Imigration.Web.Controllers
{
    public class HomeController : BaseController
    {
        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}
        #region Ctor
        private readonly IQuestionService _questionService;
        public HomeController(IQuestionService questionService)
        {
                _questionService = questionService;
        }
        #endregion

        public async Task<IActionResult> Index()
        {
            var options = new FilterQuestionViewModel
            {
                TakeEntity = 10,
                Sort = FilterQuestionSortEnum.NewToOld
            };

            ViewData["Questions"] = await _questionService.FilterQuestions(options);
            
            return View();
        }
        #region Editor Upload

        public async Task<IActionResult> UploadEditorImage(IFormFile upload)
        {
            var fileName = Guid.NewGuid() + Path.GetExtension(upload.FileName);

            upload.UploadFile(fileName, PathTools.EditorImageServerPath);

            return Json(new { url = $"{PathTools.EditorImagePath}{fileName}" });
        }

        #endregion



        #region 404
        [HttpGet("/404")]
        public IActionResult NotFoundPage()
        {
            return View();
        }

        #endregion
        //public IActionResult Privacy()
        //{
        //    return View();
        //}

        //    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //    public IActionResult Error()
        //    {
        //        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //    }
    }
}