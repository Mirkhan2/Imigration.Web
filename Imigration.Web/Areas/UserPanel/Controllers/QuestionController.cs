using Imigration.Application.Extensions;
using Imigration.Application.Security;
using Imigration.Application.Services.Interfaces;
using Imigration.Domains.Enums;
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
           
            var tagResult =
                await _questionService.CheckTagValidation(createQuestion.SelectedTags, HttpContext.User.GetUserId());

            if(tagResult.Status == CreateQuestionResultEnum.NotValidTag)
            {
                createQuestion.SelectedTagsJson = JsonConvert.SerializeObject(createQuestion.SelectedTags);
                createQuestion.SelectedTags = null;

                TempData[WarningMessage] = tagResult.Message;

                return View(createQuestion);
            }

            if (!ModelState.IsValid)
            {
                createQuestion.SelectedTagsJson = JsonConvert.SerializeObject(createQuestion.SelectedTags);
                createQuestion.SelectedTags = null;
               
                TempData[WarningMessage] = "information nicht Valid ";
                return View(createQuestion);    

            }
            createQuestion.UserId = HttpContext.User.GetUserId();
            var result = await _questionService.CreateQuestion(createQuestion);

            if (result)
            {
                TempData[SuccessMessage] = " succsed";
                return Redirect("/");
            }
            createQuestion.SelectedTagsJson = JsonConvert.SerializeObject(createQuestion.SelectedTags);
            createQuestion.SelectedTags = null;


            return View(createQuestion);

           
        }
        #endregion

        #region Edit Question
        [HttpGet("edit-question/{id}")]
        [Authorize]
        public async Task<IActionResult> EditQuestion(long id)
        {
            var result = await _questionService.FillEditQuestionViewModel(id, User.GetUserId());

            if (result == null) return NotFound();
            

            

            return View();
        }
        [HttpPost("edit-question/{id}"),ValidateAntiForgeryToken]
        public async Task<IActionResult> EditQuestion( EditQuestionViewModel edit)
        {

            var tagResult =
                await _questionService.CheckTagValidation(edit.SelectedTags, HttpContext.User.GetUserId());

            if (tagResult.Status == CreateQuestionResultEnum.NotValidTag)
            {
                edit.SelectedTagsJson = JsonConvert.SerializeObject(edit.SelectedTags);
                edit.SelectedTags = null;

                TempData[WarningMessage] = tagResult.Message;

                return View(edit);
            }

            if (!ModelState.IsValid)
            {
                edit.SelectedTagsJson = JsonConvert.SerializeObject(edit.SelectedTags);
                edit.SelectedTags = null;

                TempData[WarningMessage] = "information nicht Valid ";
                return View(edit);

            }
            edit.UserId = HttpContext.User.GetUserId();

            var result = await _questionService.EditQuestion(edit);

            if (result)
            {
                TempData[SuccessMessage] = " succsed";
                return Redirect("/");
            }
            edit.SelectedTagsJson = JsonConvert.SerializeObject(edit.SelectedTags);
            edit.SelectedTags = null;


            return View(edit);
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
            var question = await _questionService.GetQuestionById(questionId);

            if (question == null) return NotFound();

            ViewBag.IsBookMark = false;


            if (!User.Identity.IsAuthenticated 
                && await _questionService.IsExistsQuestionInUserBookmarks(questionId, User.GetUserId()))
            {
                ViewBag.IsBookMark = true;
            }

            var userIp = Request.HttpContext.Connection.RemoteIpAddress;

            if (userIp != null) {

                await _questionService.AddViewForQuestion(userIp.ToString(), question);

            }
   

            ViewData["TageList"] = _questionService.GetTagListByQuestionId(question.Id);
         
                return View(question);
        }


        [HttpGet("question/{questionId}")]
        public async Task<IActionResult> QuestionDetailByShortLink(long questionId)
        {
            var question = await _questionService.GetQuestionById(questionId);

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

            var result = await _questionService.AnswerQuestion(answerQuestion);
            if (result)
            {
                return new JsonResult(new
                {
                    status = "Success"
                });
            }
            return new JsonResult(new {status = "Error"});

        }
        [HttpGet("EditAnswer/{answerId}")]
        [Authorize]
        public async Task<IActionResult> EditAnswer(long answerId )
        {
            var result = await _questionService.FillEditAnswerViewModel(answerId, User.GetUserId());
            if (result == null) return NotFound();

            return View(result);  
        }

        [HttpGet("EditAnswer/{answerId}"), ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> EditAnswer(EditAnswerViewModel editAnswerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(editAnswerViewModel);
            }
            editAnswerViewModel.UserId = User.GetUserId();
            var result = await _questionService.EditAnswer(editAnswerViewModel);
            if (result)
            {
                TempData[SuccessMessage] = "success operation";
                return RedirectToAction("QuestionDetail", "Question", new {questionId = editAnswerViewModel.QuestionId});
            }

            TempData[ErrorMessage] = "Error has operation";
            return View();
        }

        #endregion

        #region Score Answer

        [HttpPost("ScoreUpForAnswer")]
        public async Task<IActionResult> ScoreForAnswer(long answerId)
        {
             var result = await _questionService.CreateScoreForAnswer(answerId,AnswerScoreType.Plus,User.GetUserId());
            switch (result)
            {
                case CreateScoreForAnswerResult.Error:
                    return new JsonResult(new { status = "Error" });
                    
                case CreateScoreForAnswerResult.NOtEnumScoreForDown:
                    return new JsonResult(new { status = "NOtEnoughScoreForDown" });

                   
                case CreateScoreForAnswerResult.NOtEnumScoreForUp:
                    return new JsonResult(new { status = "NOtEnoughScoreForUp" });

                  
                case CreateScoreForAnswerResult.UserCreateScoreBefore:
                    return new JsonResult(new { status = "NOtEnoughScoreForBefore" });

                  
                case CreateScoreForAnswerResult.Success:
                    return new JsonResult(new { status = "Success" });
                default:
                    throw new ArgumentOutOfRangeException();
                  
            }
        }
        [HttpPost("ScoreDownForAnswer")]
        public async Task<IActionResult> ScoreDownForAnswer(long answerId)
        {
            var result = await _questionService.CreateScoreForAnswer(answerId, AnswerScoreType.Plus, User.GetUserId());
            switch (result)
            {
                case CreateScoreForAnswerResult.Error:
                    return new JsonResult(new { status = "Error" });
                 
                case CreateScoreForAnswerResult.NOtEnumScoreForDown:
                    return new JsonResult(new { status = "NOtEnoughScoreForDown" });

                case CreateScoreForAnswerResult.NOtEnumScoreForUp:
                    return new JsonResult(new { status = "NOtEnoughScoreForUp" });

                case CreateScoreForAnswerResult.UserCreateScoreBefore:
                    return new JsonResult(new { status = "NOtEnoughScoreForBefore" });

                case CreateScoreForAnswerResult.Success:
                    return new JsonResult(new { status = "Success" });
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #endregion

        #region Score Question

        [HttpPost("ScoreUpForQuestion")]
        public async Task<IActionResult> ScoreUpForQuestion(long questionId)
        {
            var result = await _questionService.CreateScoreForQuestion(questionId, QuestionScoreType.Plus, User.GetUserId());

            switch (result)
            {
                case CreateScoreForAnswerResult.Error:
                    return new JsonResult(new { status = "Error" });

                case CreateScoreForAnswerResult.NOtEnumScoreForDown:
                    return new JsonResult(new { status = "NOtEnoughScoreForDown" });


                case CreateScoreForAnswerResult.NOtEnumScoreForUp:
                    return new JsonResult(new { status = "NOtEnoughScoreForUp" });


                case CreateScoreForAnswerResult.UserCreateScoreBefore:
                    return new JsonResult(new { status = "NOtEnoughScoreForBefore" });


                case CreateScoreForAnswerResult.Success:
                    return new JsonResult(new { status = "Success" });
                default:
                    throw new ArgumentOutOfRangeException();

            }
        }
        [HttpPost("ScoreDownForQuestion")]
        public async Task<IActionResult> ScoreDownForQuestion(long questionId)
        {
            var result = await _questionService.CreateScoreForQuestion(questionId, QuestionScoreType.Minus, User.GetUserId());
            switch (result)
            {
                case CreateScoreForAnswerResult.Error:
                    return new JsonResult(new { status = "Error" });

                case CreateScoreForAnswerResult.NOtEnumScoreForDown:
                    return new JsonResult(new { status = "NOtEnoughScoreForDown" });

                case CreateScoreForAnswerResult.NOtEnumScoreForUp:
                    return new JsonResult(new { status = "NOtEnoughScoreForUp" });

                case CreateScoreForAnswerResult.UserCreateScoreBefore:
                    return new JsonResult(new { status = "NOtEnoughScoreForBefore" });

                case CreateScoreForAnswerResult.Success:
                    return new JsonResult(new { status = "Success" });
                default:
                    throw new ArgumentOutOfRangeException();
            }
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
            if (await _questionService.HasUserAccessToSelectTrueAnswer(User.GetUserId(), answerId))
            {
                return new JsonResult(new { status = " NOtAccess" });
            }
            await _questionService.SelectTrueAnswer(User.GetUserId(),answerId);
            return new JsonResult(new { status = " SUCCESS" });
        }

        #endregion

        #region add question to BookMark
        [HttpPost("AddQuestionToBookmark")]
        public async Task<IActionResult> AddQuestionToBookMark(long questionId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return new JsonResult(new { status = "NotAuthorized" });
            }
            var result = await _questionService.AddQuestionToBookmark(questionId,User.GetUserId());
            if (!result)
            {
                return new JsonResult(new { status = " Error" });
            }

            return new JsonResult(new { status = " Success" });

        }
        #endregion

    }
}
