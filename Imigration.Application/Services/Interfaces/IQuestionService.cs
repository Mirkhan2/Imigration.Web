using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imigration.Domains.Entities.Questions;
using Imigration.Domains.Entities.Tags;
using Imigration.Domains.ViewModels.Question;

namespace Imigration.Application.Services.Interfaces
{
    public interface IQuestionService
    {
        #region Tags
        Task<List<Tag>> GetAllTags();

        Task<IQueryable<Tag>> GetAllTagsAsQueryable();
        Task<CreateQuestionResult> CheckTagValidation(List<string>? tags , long userId);

        Task<bool> CreateQuestion(CreateQuestionViewModel createQuestion);
        Task<FilterTagViewModel> FilterTags(FilterTagViewModel filter);
        Task<List<string>> GetTagListByQuestionId(long questionId);

        //Task<Question> GetQuery

        #endregion

        #region QUestions
        Task<FilterQuestionViewModel> FilterQuestions(FilterQuestionViewModel filter);

        Task<Question> GetQUestionById(long id);

        Task<bool> AnwerQuestion(AnswerQuestionViewModel answerQuestion);

        #endregion
        #region Answwer
        Task<List<Answer>> GetAllQuestionAnswers(long questionId);
        #endregion
    }
}
