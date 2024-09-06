using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imigration.Domains.Entities.Questions;
using Imigration.Domains.Entities.Tags;

namespace Imigration.Domains.Interfaces
{
    public interface IQuestionRepository
    {
        #region Tag
        Task<List<Tag>> GetAllTags();
        Task<IQueryable<Tag>> GetAllTagsAsQueryable();
        Task<bool> IsExitTagByName(string name);
        Task<Tag> GetTagByName(string name);
        Task<bool> CheckUserRequestForTag(long userId, string tag);
        Task AddRequestTag(RequestTag tag);
        Task SaveChanges();
        Task<int> RequestCountForTag(string tag);
        Task<List<string>> GetTagListByQuestionId(long questionId); 
        Task AddTag(Tag tag);
        Task UpdateTag(Tag tag);

        #endregion

        #region Question
        Task AddQuestion(Question question);
        Task UpdateQuestion(Question question);
        Task<IQueryable<Question>> GetAllQuestions();
        Task<Question> GetQUestionById(long id);    
        Task<bool> IsExistsViewForQuestion( string userIp, long questionId );



        #endregion

        #region View

        Task<bool> IsExistsViewForQuestion(string userIp, long questionId);
        Task AddQuestionView(QuestionView view);

        #endregion

        #region Selected Tag
        Task AddSelectedQuestionTag(SelectQuestionTag selectQuestionTag);
        #endregion

        #region Answer

        Task AddAnswer(Answer answer);
        Task UpdateAnswer(Answer answer);
        Task<List<Answer>> GetAllQuestionAnswers(long questionId );
        Task<Answer?> GetAnswerById(long id);

        #endregion
    }
}
