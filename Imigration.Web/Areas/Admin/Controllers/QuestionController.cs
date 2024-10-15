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

        public async Task<IActionResult> QuestionList(FilterQuestionViewModel filter)
        {
            var result = await _questionService.FilterQuestions(filter);

            return View(result);
        }

    }
}
