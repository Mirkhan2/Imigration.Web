using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imigration.DataLayer.Context;
using Imigration.Domains.Entities.Questions;
using Imigration.Domains.Entities.Tags;
using Imigration.Domains.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Imigration.DataLayer.Repositories
{
    public class QuestionRepository : IQuestionRepository
    {
        #region Ctor
        private readonly ImigrationDbContext _context;
        public QuestionRepository(ImigrationDbContext context)
        {
            _context = context;
        }

        #endregion


        #region TAgs

        public async Task<List<Tag>> GetAllTags()
        {
            return await _context.Tags.Where(s => !s.IsDelete).ToListAsync();
        }

        #endregion

    }
}
