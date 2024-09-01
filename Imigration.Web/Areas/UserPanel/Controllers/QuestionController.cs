using Imigration.Application.Services.Interfaces;
using Imigration.Domains.ViewModels.Question;
using Imigration.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Imigration.Web.Areas.UserPanel.Controllers
{
    public class QuestionController : BaseController
    {
        #region Ctor
        private  readonly IQuestionService _questionService;
        private bool createQuestion;

        public QuestionController(IQuestionService questionService)
        {
            _questionService = questionService;
        }
      
        #endregion

        #region Create Question
        [Authorize]
        [HttpGet("create-question")]
        public async Task<IActionResult> CreateQuestion()
        {
            return View();
        }

        [Authorize]
        [HttpPost("create-question"),ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateQuestion(CreateQuestionViewModel createQuestion)
        {
            if(createQuestion.SelectedTags == null || !createQuestion.SelectedTags.Any(){
                TempData[WarningMessage] = "Tagsmis neseccary";
            }
            return View();
        }
        #endregion

        #region Get Tags

        [HttpGet("get-tags")]
        public async Task<IActionResult> GetTagsForSuggest(string name )
        {
            var tags = await _questionService.GetAllTags();

            var filteredTags = tags.Where(s => s.Title.Contains(name))
                .Select(s => s.Title)
                .ToList();

            return Json(filteredTags);
        }

        #endregion
    }
}
