using Imigration.Application.Extensions;
using Imigration.Application.Security;
using Imigration.Application.Services.Interfaces;
using Imigration.Domains.ViewModels.Question;
using Imigration.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Imigration.Web.Areas.UserPanel.Controllers
{
    public class QuestionController : BaseController
    {
        #region Ctor
        private  readonly IQuestionService _questionService;


        public QuestionController(IQuestionService questionService)
        {
            _questionService = questionService;
        }
      
        #endregion

        #region Create Question
        [Authorize]
        [HttpGet("create-question")]
        public async Task<IActionResult> CreateQuestion ()
        {
            return View();
        }

        [Authorize]
        [HttpPost("create-question"),ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateQuestion(CreateQuestionViewModel createQuestion)
        {
           
            var tagResult = await _questionService.CheckTagValidation(createQuestion.SelectedTags, HttpContext.User.GetUserId());

            if(tagResult.Status == CreateQuestionResultEnum.NotValidTag)
            {
                createQuestion.SelectedTagsJson = JsonConvert.SerializeObject(createQuestion.SelectedTags);
                createQuestion.SelectedTags = null;

                TempData[WarningMessage] = tagResult.Message;

                return View(createQuestion);
            }
            createQuestion.UserId = HttpContext.User.GetUserId();
            var result = await _questionService.CreateQuestion(createQuestion);
            if (result)
            {
                TempData[SuccessMessage] = " succsed";
                return Redirect("/");
            }

            createQuestion.SelectedTagsJson =  JsonConvert.SerializeObject(createQuestion.SelectedTags);
            createQuestion.SelectedTags = null;

            return View();
        }
        #endregion

        #region Get Tags

        [HttpGet("get-tags")]
        public async Task<IActionResult> GetTagsForSuggest(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return Json(null);

            }
            var tags = await _questionService.GetAllTags();

            var filteredTags = tags.Where(s => s.Title.Contains(name))
                .Select(s => s.Title)
                .ToList();

            return Json(filteredTags);
        }

        #endregion

        #region Question List

        [HttpGet("questions")]
        public async Task<IActionResult> QuestionList(FilterQuestionViewModel filter)
        {
            filter.TakeEntity = 1;

            var result = await _questionService.FilterQuestions(filter);

            return View(result);
        }

        #endregion

        #region Filter Question ByTag

        [HttpGet("tags/{tagName}")]
        public async Task<IActionResult> FilterTags(FilterTagViewModel filter)
        {
           
            var result = await _questionService.FilterTags(filter);

            

            return View(result);
        }
        #endregion

        #region Question detial

        [HttpGet("question/{questionId}")]
        public async Task<IActionResult> QuestionDetail(long  questionId)
        {
            var question = await _questionService.GetQUestionById(questionId);

            if (question == null) return NotFound();

            var userIp = Request.HttpContext.Connection.RemoteIpAddress;

            if (userIp != null) {

                await _questionService.AddViewFormQuestion(userIp.ToString(), question);

            }
   

            ViewData["TageList"] = _questionService.GetTagListByQuestionId(question.Id);
         
                return View(question);
        }


        [HttpGet("question/{questionId}")]
        public async Task<IActionResult> QuestionDetailByShortLink(long questionId)
        {
            var question = await _questionService.GetQUestionById(questionId);

            if (question == null) return NotFound();

            return RedirectToAction("QuestionDetail", "Question", new { questionId = questionId });
        }

        [HttpPost]
        [Authorize]
        public  async Task<IActionResult> AnwerQuestion(AnswerQuestionViewModel answerQuestion)
        {
            if (string.IsNullOrEmpty(answerQuestion.Answer))
            {
                return new JsonResult(new { status = "EmptyAnswer" });
            }
            answerQuestion.UserId = User.GetUserId();

            var result = await _questionService.AnwerQuestion(answerQuestion);
            if (result)
            {
                return new JsonResult(new
                {
                    status = "Success"
                });
            }
            return new JsonResult(new {status = "Error"});

        }
        #endregion

        #region select True Answer

        [HttpPost("SelectTrueAnswer")]
        public  async Task<IActionResult> SelectTrueAnswer(long answerId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return new JsonResult(new { status = " NOAuthorize" });
            }
            if (await _questionService.HasYserAccessToSelectTrueAnswer(User.GetUserId(), answerId))
            {
                return new JsonResult(new { status = " NOtAccess" });
            }
            await _questionService.SelectTrueAnswer(User.GetUserId(),answerId);
            return new JsonResult(new { status = " SUCCESS" });
        }

        #endregion

    }
}
