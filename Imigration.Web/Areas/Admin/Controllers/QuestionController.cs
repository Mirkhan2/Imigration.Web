using Imigration.Application.Services.Interfaces;
using Imigration.Domains.ViewModels.Question;
using Microsoft.AspNetCore.Mvc;

namespace Imigration.Web.Areas.Admin.Controllers
{
    public class QuestionController : AdminBaseController
    {
        #region Ctor

        private readonly IQuestionService _questionService;

        public QuestionController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        #endregion

        #region Question List
        public async Task<IActionResult> QuestionsList(FilterQuestionViewModel filter)
        {

            var result = await _questionService.FilterQuestions(filter);

            return View(result);
        }
        #endregion

        #region DeleteQuestion
        [HttpPost]
        public async Task<IActionResult> DeleteQuestion(long id)
        {
            var result = await _questionService.DeleteQuestion(id);

            if (!ModelState.IsValid)
            {
                return new JsonResult(new { status = " error", message = "nicht guldig" });
            }

            return new JsonResult(new { status = " sucess", message = " guldig" });

        }
        #endregion

        #region IsCheckedQuestion
        [HttpPost]
        public async Task<IActionResult> ChangeIsCheckedQuestion(long id)
        {
            var result = await _questionService.ChangeQuestionIschecked(id);

            if (!ModelState.IsValid)
            {
                return new JsonResult(new { status = " error", message = "nicht guldig" });
            }

            return new JsonResult(new { status = " sucess", message = " guldig" });

        }
        #endregion
    }
}
