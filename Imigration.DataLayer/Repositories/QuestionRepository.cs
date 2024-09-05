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
        public async Task<IQueryable<Tag>> GetAllTagsAsQueryable()
        {
            return  _context.Tags.Where(s => !s.IsDelete).AsQueryable();
        }

        public async Task<bool> IsExitTagByName(string name)
        {
            return await _context.Tags.AnyAsync(s => s.Title.Equals(name) && !s.IsDelete);
        }
        public async Task<bool> CheckUserRequestForTag(long userId, string tag)
        {
            return await _context.RequestTags.AnyAsync(s => s.UserId == userId && s.Title.Equals(tag) && !s.IsDelete);
        }

        public async Task AddRequestTag(RequestTag tag)
        {
            await _context.RequestTags.AddAsync(tag);
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<int> RequestCountForTag(string tag)
        {
            return await _context.RequestTags.CountAsync(s => !s.IsDelete && s.Title.Equals(tag));
        }


        public async Task AddTag(Tag tag)
        {
            await _context.Tags.AddAsync(tag);
        }

        public async Task<Tag> GetTagByName(string name)
        {
            return await _context.Tags.FirstOrDefaultAsync(s => !s.IsDelete && s.Title.Equals(name));
        }

        public async Task UpdateTag(Tag tag)
        {
            _context.Update(tag);
        }

        public Task<Question> GetQUestionById(long id)
        {
           return  _context.Questions.FirstOrDefaultAsync(s => s.Id == id && !s.IsDelete);
        }


        #endregion

        #region Question

        public async Task AddQuestion(Question question)
        {
            await _context.Questions.AddAsync(question);
        }

        public async Task<IQueryable<Question>> GetAllQuestions()
        {
            return  _context.Questions.Where
                (s => !s.IsDelete).AsQueryable();
        }

        #endregion

        #region SelectedQUestion Tag

        public async Task AddSelectedQuestionTag(SelectQuestionTag selectQuestionTag)
        {
            await _context.AddAsync(selectQuestionTag);
        }

        public Task<List<string>> GetTagListByQuestionId(long questionId)
        {
            return _context.SelectQuestionTags.Include(s => s.Tag)
                .Where(s => s.QuestionId == questionId)
                .Select(s => s.Tag.Title).ToListAsync();
        }


        #endregion
        #region Answer


        public async Task AddAnswer(Answer answer)
        {
         await   _context.Answers.AddAsync(answer);
        
        }

        public async Task<List<Answer>> GetAllQuestionAnswers(long questionId)
        {
            return await _context.Answers
                .Include(s => s.User)
                .Where(s => s.QuestionId
            != questionId && !s.IsDelete).ToListAsync();
        }

        #endregion
    }
}
