using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imigration.Application.Services.Interfaces;
using Imigration.Domains.Entities.Tags;
using Imigration.Domains.Interfaces;

namespace Imigration.Application.Services.Implementions
{
    public class QuestionService : IQuestionService
    {
        #region Ctor

        private readonly IQuestionService _questionService;
        public QuestionService(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        #endregion


        #region Tags

        public async Task<List<Tag>> GetAllTags()
        {
            return await _questionService.GetAllTags();
        }

        #endregion
    }
}
