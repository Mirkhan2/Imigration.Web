using Imigration.Application.Services.Implementions;
using Imigration.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Imigration.Web.ViewComponents
{
    public class QuestionAnswersListViewComponent : ViewComponent
    {

        #region ctor

        private readonly IQuestionService _questionService;
        public QuestionAnswersListViewComponent(IQuestionService questionService)
        {
            _questionService = questionService;
        }
       
        #endregion

        public async Task<IViewComponentResult> InvokeAsync(long questionId)
        {
        //    var user = await _userService.GetUserById(HttpContext.User.GetUserId());

            var answers = await _questionService.GetAllQuestionAnswers(questionId);
            return View("QuestionAnswersList" , answers);
        }
    }
}
