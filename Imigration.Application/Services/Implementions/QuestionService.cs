using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imigration.Application.Security;
using Imigration.Application.Services.Interfaces;
using Imigration.Domains.Entities.Account;
using Imigration.Domains.Entities.Questions;
using Imigration.Domains.Entities.Tags;
using Imigration.Domains.Interfaces;
using Imigration.Domains.ViewModels.Common;
using Imigration.Domains.ViewModels.Question;
using Microsoft.Extensions.Options;

namespace Imigration.Application.Services.Implementions
{
    public class QuestionService : IQuestionService
    {
        #region Ctor

        private readonly IQuestionRepository _questionRepository;
        private ScoreManagementViewModel _scoreManagement;
        private IUserService _userService;

        public QuestionService(IQuestionRepository questionRepository, IOptions<ScoreManagementViewModel> scoreManagement,IUserService userService)
        {
            _questionRepository = questionRepository;
            _scoreManagement = scoreManagement.Value;
            _userService = userService;
        }


        #endregion


        #region Tags

        public async Task<List<Tag>> GetAllTags()
        {
            //var minCOunt = _scoreManagement.MinRequestsCountForVerifyTag;
            return await _questionRepository.GetAllTags();
        }

        public async Task<CreateQuestionResult> CheckTagValidation(List<string> tags, long userId)
        {
            if (tags != null && tags.Any())
            {
                foreach (var tag in tags)
                {
                    var isExistTag = await _questionRepository.IsExitTagByName(tag.SanitizeText().Trim().ToLower());

                    if (isExistTag) continue;

                    var isUserRequestedForTag =
                        await _questionRepository.CheckUserRequestForTag(userId, tag.SanitizeText().Trim().ToLower());

                    if (isUserRequestedForTag)
                    {
                        return new CreateQuestionResult
                        {
                            Status = CreateQuestionResultEnum.NotValidTag,
                            Message = $"{tag} zooori dave {_scoreManagement.MinRequestsCountForVerifyTag}tag"
                        };
                    }
                    var tagRequest = new RequestTag
                    {
                        Title = tag.SanitizeText().ToLower(),
                        UserId = userId,
                    };
                    await _questionRepository.AddRequestTag(tagRequest);
                    await _questionRepository.SaveChanges();

                    var requestCount =
                        await _questionRepository.RequestCountForTag(tag.SanitizeText().Trim().ToLower());
                        
                    if (requestCount < _scoreManagement.MinRequestsCountForVerifyTag)
                    {
                        return new CreateQuestionResult
                        {
                            Status = CreateQuestionResultEnum.NotValidTag,
                            Message = $"{tag} zooori dave {_scoreManagement.MinRequestsCountForVerifyTag}tag"
                        };
                    }

                    var newTag = new Tag
                    {
                        Title = tag.SanitizeText().Trim().ToLower(),
                    };
                    await _questionRepository.AddTag(newTag);
                    await _questionRepository.SaveChanges();

                }
                return new CreateQuestionResult
                {
                    Status = CreateQuestionResultEnum.Success,
                    Message = " Can be Valid tags"
                };

            }
            return new CreateQuestionResult
            {
                Status = CreateQuestionResultEnum.NotValidTag,
                Message = " Can be EMpty tags"
            };
        }

        public async Task<bool> CreateQuestion(CreateQuestionViewModel createQuestion)
        {
            var question = new Question()
            {
                Content = createQuestion.Description.SanitizeText(),
                Title = createQuestion.Title.SanitizeText(),
                UserId = createQuestion.UserId,

            };
            await _questionRepository.AddQuestion(question);
            await _questionRepository.SaveChanges();


            if (createQuestion.SelectedTags !=null && createQuestion.SelectedTags.Any())
            {
                foreach (var questionSelectedTag in createQuestion.SelectedTags)
                {
                    var tag= _questionRepository.GetTagByName(questionSelectedTag.SanitizeText().Trim().ToLower());
                    if (tag == null) continue;
                    

                    

                    var selectedTag = new SelectQuestionTag()
                    {
                        QuestionId =  question.Id,
                          TagId = tag.Id,


                    };
                    await _questionRepository.AddSelectedQuestionTag(selectedTag);

                }
                await _questionRepository.SaveChanges();
            }

            await _userService.UpdateUserScoreAndMedal(createQuestion.UserId, _scoreManagement.AddNewQuestionScrore);

            return true;
        }

        #endregion
    }
}
