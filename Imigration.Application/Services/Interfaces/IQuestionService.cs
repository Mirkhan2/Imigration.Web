using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imigration.Domains.Entities.Tags;
using Imigration.Domains.ViewModels.Question;

namespace Imigration.Application.Services.Interfaces
{
    public interface IQuestionService
    {
        #region Tags
        Task<List<Tag>> GetAllTags();

        Task<CreateQuestionResult> CheckTagValidation(List<string>? tags , long userId);

        Task<bool> CreateQuestion(CreateQuestionViewModel createQuestion);


        #endregion

        #region QUestions
        Task<FilterQuestionViewModel> FilterQuestions(FilterQuestionViewModel filter);
        #endregion
    }
}
