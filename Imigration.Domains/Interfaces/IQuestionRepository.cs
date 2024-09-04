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
        Task<bool> IsExitTagByName(string name);
        Task<Tag> GetTagByName(string name);
        Task<bool> CheckUserRequestForTag(long userId, string tag);
        Task AddRequestTag(RequestTag tag);
        Task SaveChanges();
        Task<int> RequestCountForTag(string tag);
        Task AddTag(Tag tag);

        #endregion

        #region Question
        Task AddQuestion(Question question);
        Task<IQueryable<Question>> GetAllQuestions();

        #endregion
        #region Selected Tag
        Task AddSelectedQuestionTag(SelectQuestionTag selectQuestionTag);
        #endregion
        //Task DeleteQuestion(int id );
        //Task<Question> GetQuestionById(long id);
        //Task<Question> GetAnswerByid(long id);
        //Task<Question> GetQuestionType(string Question);
    }
}
