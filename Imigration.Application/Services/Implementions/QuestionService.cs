using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imigration.Application.Extensions;
using Imigration.Application.Security;
using Imigration.Application.Services.Interfaces;
using Imigration.Domains.Entities.Account;
using Imigration.Domains.Entities.Questions;
using Imigration.Domains.Entities.Tags;
using Imigration.Domains.Interfaces;
using Imigration.Domains.ViewModels.Common;
using Imigration.Domains.ViewModels.Question;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using static Imigration.Domains.ViewModels.Question.FilterQuestionViewModel;

namespace Imigration.Application.Services.Implementions
{
    public class QuestionService : IQuestionService
    {
        #region Ctor

        private readonly IQuestionRepository _questionRepository;
        private ScoreManagementViewModel _scoreManagement;
        private IUserService _userService;

        public QuestionService(IQuestionRepository questionRepository, IOptions<ScoreManagementViewModel> scoreManagement, IUserService userService)
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


            if (createQuestion.SelectedTags != null && createQuestion.SelectedTags.Any())
            {
                foreach (var questionSelectedTag in createQuestion.SelectedTags)
                {
                    var tag = _questionRepository.GetTagByName(questionSelectedTag.SanitizeText().Trim().ToLower());
                    if (tag == null) continue;




                    var selectedTag = new SelectQuestionTag()
                    {
                        QuestionId = question.Id,
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

        #region Questions
        public async Task<FilterQuestionViewModel> FilterQuestions(FilterQuestionViewModel filter)
        {

            var query = await _questionRepository.GetAllQuestions();

            #region Filter By Tag

            if (!string.IsNullOrEmpty(filter.TagTitle))
            {

                   query = query.Include(s => s.SelectQuestionTags)
                    .ThenInclude(s => s.Tag)
                    .Where(s => s.SelectQuestionTags.Any(a => a.Tag.Title.Equals(filter.TagTitle)));

            }

            #endregion

            if (!string.IsNullOrEmpty(filter.Title))
            {
                query = query.Where(s => s.Title.Contains(filter.Title.SanitizeText().Trim()));
            }

            switch (filter.Sort)
            {
                case FilterQuestionSortEnum.NewToOld:
                    query = query.OrderByDescending(s => s.CreateDate);
                    break;
                case FilterQuestionSortEnum.OldToNew:
                    query = query.OrderBy(s => s.CreateDate);
                    break;
                case FilterQuestionSortEnum.ScoreHighToLow:
                    query = query.OrderByDescending(s => s.Score);
                    break;
                case FilterQuestionSortEnum.ScoreLowToHigh:
                    query = query.OrderBy(s => s.Score);
                    break;
            }

            var result = query
                .Include(s => s.Answers)
                .Include(s => s.SelectQuestionTags)
                .ThenInclude(a => a.Tag)
                .Include(s => s.User)
                .Select(s => new QuestionListViewModel()
                {
                    AnswersCount = s.Answers.Count(a => !a.IsDelete),
                    HasAnyAnswer = s.Answers.Any(a => !a.IsDelete),
                    HasAnyTrueAnswer = s.Answers.Any(a => !a.IsDelete && a.IsTrue),
                    QuestionId = s.Id,
                    Score = s.Score,
                    Title = s.Title,
                    ViewCount = s.ViewCount,
                    UserDisplayName = s.User.GetUserDisplayName(),
                    Tags = s.SelectQuestionTags.Where(a => !a.Tag.IsDelete).Select(a => a.Tag.Title).ToList(),
                    AnswerByDisplayName = s.Answers.Any(a => !a.IsDelete) ? s.Answers.OrderByDescending(a => a.CreateDate).First().User.GetUserDisplayName() : null,
                    CreateDate = s.CreateDate.TimeAgo(),
                    AnswerByCreateDate = s.Answers.Any(a => !a.IsDelete) ? s.Answers.OrderByDescending(a => a.CreateDate).First().CreateDate.TimeAgo() : null
                }).AsQueryable();

            await filter.SetPaging(result);

            return filter;
        }
    }

        #endregion
  }
