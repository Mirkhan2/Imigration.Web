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
        #endregion

        //Task DeleteQuestion(int id );
        //Task<Question> GetQuestionById(long id);
        //Task<Question> GetAnswerByid(long id);
        //Task<Question> GetQuestionType(string Question);
    }
}
