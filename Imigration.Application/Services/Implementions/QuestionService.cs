
using System.Text.Json.Serialization;
using Imigration.Application.Extensions;
using Imigration.Application.Security;
using Imigration.Application.Services.Interfaces;
using Imigration.Application.Statics;
using Imigration.DataLayer.Repositories;
using Imigration.Domains.Entities.Account;
using Imigration.Domains.Entities.Questions;
using Imigration.Domains.Entities.Tags;
using Imigration.Domains.Enums;
using Imigration.Domains.Interfaces;
using Imigration.Domains.ViewModels.Admin.Tag;
using Imigration.Domains.ViewModels.Common;
using Imigration.Domains.ViewModels.Question;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
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

        public async Task<List<string>> GetTagListByQuestionId(long questionId)
        {
            return await _questionRepository.GetTagListByQuestionId(questionId);
        }
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
                    var isExistTag = await _questionRepository.IsExistsTagByName(tag.SanitizeText().Trim().ToLower());

                    if (isExistTag) continue;

                    var isUserRequestedForTag =
                        await _questionRepository.CheckUserRequestedForTag(userId, tag.SanitizeText().Trim().ToLower());

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
                    var tag = await _questionRepository.GetTagByName(questionSelectedTag.SanitizeText().Trim().ToLower());
                    if (tag == null) continue;

                    tag.UseCount += 1;
                    await _questionRepository.UpdateTag(tag);



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
        public async Task<FilterTagViewModel> FilterTags(FilterTagViewModel filter)
        {
            var query = await _questionRepository.GetAllTagsAsQueryable();

            if (!string.IsNullOrEmpty(filter.Title))
            {
                query = query.Where(s => s.Title.Contains(filter.Title));
            }
            switch (filter.Sort)
            {
                case FilterTagEnum.NewToOld:
                    query = query.OrderByDescending(s => s.CreateDate);
                    break;
                case FilterTagEnum.OldToNew:
                    query = query.OrderBy(s => s.CreateDate);
                    break;
                case FilterTagEnum.UserCountHighToLow:
                    query = query.OrderByDescending(s => s.UseCount);
                    break;
                case FilterTagEnum.UseCountLOwToHigh:
                    query = query.OrderBy(s => s.UseCount);

                    break;

            }
            await filter.SetPaging(query);

            return filter;
        }


        #endregion

        #region Questions
        public async Task<bool> EditQuestion(EditQuestionViewModel edit)
        {
            var question = await _questionRepository.GetQuestionById(edit.UserId);

            if (question != null) return false;

            var user = await _userService.GetUserById(edit.Id);

            if (user == null) return false;

            if (question.UserId != edit.UserId && !user.IsAdmin)
            {
                return false;
            }
            FileExtensions.ManageEditorImages(question.Content, edit.Description, PathTools.EditorImagePath);
            //var res = edit.Description.GetSrcValue();

            question.Title = edit.Title;
            question.Content = edit.Description;

            #region Delete CUrrent Tags
            var currenTags = question.SelectQuestionTags.ToList();
            foreach (var tag in currenTags)
            {
                _questionRepository.RemoveSelectQuestionTag(tag);
            }
            #endregion

            #region Add New Tags

            if (edit.SelectedTags != null && edit.SelectedTags.Any())
            {
                foreach (var questionSelectedTag in edit.SelectedTags)
                {
                    var tag = await _questionRepository.GetTagByName(questionSelectedTag.SanitizeText().Trim().ToLower());

                    if (tag != null) continue;
                    tag.UseCount += 1;
                    await _questionRepository.UpdateTag(tag);

                    var selectedTag = new SelectQuestionTag()
                    {
                        QuestionId = question.Id,
                        TagId = tag.Id,
                    };
                    await _questionRepository.AddSelectedQuestionTag(selectedTag);
                }
                await _questionRepository.SaveChanges();
            }

            #endregion
            return true;

        }


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


        public async Task<Question> GetQuestionById(long id)
        {
            return await _questionRepository.GetQuestionById(id);
        }

        public Task<IQueryable<Tag>> GetAllTagsAsQueryable()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> AnswerQuestion(AnswerQuestionViewModel answerQuestion)
        {
            var question = await GetQuestionById(answerQuestion.QuestionId);

            if (question == null) return false;


            var answer = new Answer()
            {
                Content = answerQuestion.Answer.SanitizeText(),
                QuestionId = answerQuestion.QuestionId,
                UserId = answerQuestion.UserId,

            };
            await _questionRepository.AddAnswer(answer);
            await _questionRepository.SaveChanges();

            await _userService.UpdateUserScoreAndMedal(answerQuestion.UserId, _scoreManagement.AddNewQuestionScrore);

            return true;
        }

        public async Task<List<Answer>> GetAllQuestionAnswers(long questionId)
        {
            return await _questionRepository.GetAllQuestionAnswers(questionId);
        }

        public async Task AddViewForQuestion(string userIp, Question question)
        {
            if (await _questionRepository.IsExistsViewForQuestion(userIp, question.Id))
            {
                return;
            }
            var view = new QuestionView()
            {
                QuestionId = question.Id,
                UserIP = userIp
            };

            await _questionRepository.AddQuestionView(view);


            question.ViewCount += 1;

            await _questionRepository.UpdateQuestion(question);

            await _questionRepository.SaveChanges();
        }

        public async Task<bool> AddQuestionToBookmark(long questionId, long userId)
        {
            var question = await GetQuestionById(questionId);

            if (question == null) return false;

            if (await _questionRepository.IsExistsQuestionInUserBookmarks(questionId, userId))
            {
                var bookmark = await _questionRepository.GetBookmarkByQuestionAndUserId(questionId, userId);
                if (bookmark == null) return false;

                _questionRepository.RemoveBookmark(bookmark);
            }
            else
            {
                var newbookmark = new UserQuestionBookmark
                {
                    QuestionId = questionId,
                    UserId = userId
                };
                await _questionRepository.AddBookmark(newbookmark);
            }

            await _questionRepository.SaveChanges();

            return true;

        }

        public async Task<bool> IsExistsQuestionInUserBookmarks(long userId, long questionId)
        {
            return await _questionRepository.IsExistsQuestionInUserBookmarks(questionId, userId);
        }

        public async Task<EditQuestionViewModel?> FillEditQuestionViewModel(long questionId, long userId)
        {
            var question = await GetQuestionById(questionId);

            if (question == null) return null;

            var user = await _userService.GetUserById(userId);
            if (user == null) return null;

            if (question.UserId != user.Id && !user.IsAdmin)
            {
                return null;
            }

            var tags = await GetTagListByQuestionId(questionId);

            var result = new EditQuestionViewModel
            {
                Id = question.Id,
                Description = question.Content,
                Title = question.Title,
                SelectedTagsJson = JsonConvert.SerializeObject(tags)

            };
            return result;
        }





        #endregion

        #region Answer
        public async Task<bool> HasUserAccessToSelectTrueAnswer(long userId, long answerId)
        {
            var answer = await _questionRepository.GetAnswerById(answerId);

            if (answer == null) return false;

            var user = await _userService.GetUserById(userId);

            if (user == null) return false;

            if (user.IsAdmin) return true;

            if (answer.Question.UserId != userId)
            {
                return false;
            }

            return true;


        }

        public async Task SelectTrueAnswer(long userid, long answerId)
        {
            var answer = await _questionRepository.GetAnswerById(answerId);

            if (answer == null) return;
            answer.IsTrue = !answer.IsTrue;

            await _questionRepository.UpdateAnswer(answer);
            await _questionRepository.SaveChanges();

        }

        public async Task<CreateScoreForAnswerResult> CreateScoreForAnswer(long answerId, AnswerScoreType type, long userId)
        {
            var answer = await _questionRepository.GetAnswerById(answerId);

            if (answer == null) return CreateScoreForAnswerResult.Error;

            var user = await _userService.GetUserById(userId);
            if (user == null) return CreateScoreForAnswerResult.Error;

            if (type == AnswerScoreType.Minus && user.Score < _scoreManagement.MinScoreForDownScoreAnswer)
            {
                return CreateScoreForAnswerResult.NOtEnumScoreForDown;
            }
            if (type == AnswerScoreType.Plus && user.Score < _scoreManagement.MinScoreForUpScoreAnswer)
            {
                return CreateScoreForAnswerResult.NOtEnumScoreForUp;
            }

            if (await _questionRepository.IsExistsUserScoreForQuestion(userId, answerId))
            {
                return CreateScoreForAnswerResult.UserCreateScoreBefore;
            }
            var score = new AnswerUserScore()
            {
                AnswerId = answerId,
                UserId = userId,
                Type = type

            };
            await _questionRepository.AddAnswerUserScore(score);

            if (type == AnswerScoreType.Minus)
            {
                answer.Score -= 1;
            }
            else
            {
                answer.Score += 1;
            }
            await _questionRepository.UpdateAnswer(answer);

            await _questionRepository.SaveChanges();

            return CreateScoreForAnswerResult.Success;
        }

        public async Task<CreateScoreForAnswerResult> CreateScoreForQuestion(long questionId, QuestionScoreType type, long userId)
        {
            var questions = await _questionRepository.GetAnswerById(questionId);

            if (questions == null) return CreateScoreForAnswerResult.Error;

            var user = await _userService.GetUserById(userId);

            if (user == null) return CreateScoreForAnswerResult.Error;

            if (type == QuestionScoreType.Minus && user.Score < _scoreManagement.MinScoreForDownScoreAnswer)
            {
                return CreateScoreForAnswerResult.NOtEnumScoreForDown;
            }
            if (type == QuestionScoreType.Plus && user.Score < _scoreManagement.MinScoreForUpScoreAnswer)
            {
                return CreateScoreForAnswerResult.NOtEnumScoreForUp;
            }
            if (await _questionRepository.IsExistsUserScoreForQuestion(questionId, userId))
            {
                return CreateScoreForAnswerResult.UserCreateScoreBefore;
            }

            var score = new QuestionUserScore()
            {
                QuestionId = questionId,
                Type = type,
                UserId = userId
            };

            await _questionRepository.AddQuestionUserScore(score);

            if (type == QuestionScoreType.Minus)
            {
                questions.Score -= 1;
            }
            else if (type == QuestionScoreType.Plus)
            {
                questions.Score += 1;
            }

            //   await _questionRepository.UpdateQuestion(questions);

            await _questionRepository.SaveChanges();

            return CreateScoreForAnswerResult.Success;
        }

        public async Task<EditAnswerViewModel> FillEditAnswerViewModel(long answerId, long userId)
        {
            var answer = await _questionRepository.GetAnswerById(answerId);

            if (answer == null) return null;

            var user = await _userService.GetUserById(userId);

            if (user == null) return null;

            if (answer.UserId != user.Id && !user.IsAdmin)
            {
                return null;
            }
            return new EditAnswerViewModel
            {
                Answer = answer.Content,
                AnswerId = answer.Id,
                QuestionId = answer.QuestionId,

            };
        }

        public async Task<bool> EditAnswer(EditAnswerViewModel editAnswerviewModel)
        {
            var answer = await _questionRepository.GetAnswerById(editAnswerviewModel.AnswerId);

            if (answer == null) return false;

            if (answer.QuestionId != editAnswerviewModel.QuestionId) return false;




            var user = await _userService.GetUserById(editAnswerviewModel.UserId);

            if (user == null) return false;

            if (answer.UserId != user.Id && !user.IsAdmin)
            {
                return false;
            }
            answer.Content = editAnswerviewModel.Answer;
            await _questionRepository.UpdateAnswer(answer);
            await _questionRepository.SaveChanges();

            return true;
        }


        #endregion

        #region Admin   

        public async Task<List<TagsViewModelJson>> GetTagsViewModelJson()
        {
            var tags = await _questionRepository.GetAllTagsAsQueryable();
            return tags.OrderByDescending(s => s.UseCount)
               .Take(30)
               .Select(s => new TagsViewModelJson
               {
                   Title = s.Title,
                   UseCount = s.UseCount,

               }).ToList();
        }

        public async Task<FilterTagAdminViewModel> FilterTagAdmin(FilterTagAdminViewModel filter)
        {
            var query = await _questionRepository.GetAllTagsAsQueryable();

            if (!string.IsNullOrEmpty(filter.Title))
            {
                query = query.Where(s => s.Title.Contains(filter.Title));
            }

            switch (filter.Status)
            {
                case FilterTagAdminStatus.All:
                    break;
                case FilterTagAdminStatus.HasDescription:
                    query = query.Where(s => !string.IsNullOrEmpty(s.Description));
                    break;
                case FilterTagAdminStatus.NoDescription:
                    query = query.Where(s => string.IsNullOrEmpty(s.Description));

                    break;
            }
            
            await filter.SetPaging(query);
            return filter;

        }


        #endregion

    }

}
