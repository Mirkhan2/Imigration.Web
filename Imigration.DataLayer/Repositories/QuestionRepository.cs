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
            return _context.Tags.Where(s => !s.IsDelete).AsQueryable();
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
            return _context.Questions.FirstOrDefaultAsync(s => s.Id == id && !s.IsDelete);
        }


        #endregion


        #region Question

        public async Task AddQuestion(Question question)
        {
            await _context.Questions.AddAsync(question);
        }

        public async Task<IQueryable<Question>> GetAllQuestions()
        {
            return _context.Questions.Where
                (s => !s.IsDelete).AsQueryable();
        }

        public async Task UpdateQuestion(Question question)
        {
            _context.Questions.Update(question);
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

        #region View

        public async Task<bool> IsExistsViewForQuestion(string userIp, long questionId)
        {
            return await _context.QuestionViews.AnyAsync(s => s.UserIP.Equals(userIp) && s.QuestionId == questionId);
        }

        public async Task<bool> IsExistsViewForQuestions(string userIp, long questionId)
        {
            return await _context.QuestionViews.AnyAsync(s => s.UserIP.Equals(userIp) && s.QuestionId == questionId);

        }

        public async Task AddQuestionView(QuestionView view)
        {
            await _context.QuestionViews.AddAsync(view);

        }
        #endregion

        #region Answer


        public async Task AddAnswer(Answer answer)
        {
            await _context.Answers.AddAsync(answer);

        }

        public async Task<List<Answer>> GetAllQuestionAnswers(long questionId)
        {
            return await _context.Answers
                .Include(s => s.User)
                .Where(s => s.QuestionId
            != questionId && !s.IsDelete).ToListAsync();
        }

        public async Task<Answer?> GetAnswerById(long id)
        {
            return await _context.Answers.Include(s => s.Question).FirstOrDefaultAsync(s => s.Id == id && !s.IsDelete);
        }

        public async Task UpdateAnswer(Answer answer)
        {
            _context.Answers.Update(answer);
        }

        public async Task<bool> IsExistUserScoreForAnswer(long answerId, long userId)
        {
            return await _context.AnswerUserScores.AnyAsync(s => s.AnswerId == answerId && s.UserId == userId);


        }

        public async Task AddAnswerUserScore(AnswerUserScore score)
        {
            await _context.AnswerUserScores.AddAsync(score);
        }

        public async Task<bool> IsExistUserScoreForQuestion(long questionId, long userId)
        {
            return await _context.QuestionUserScores.AddAsync(s => s.QuestionId == questionId && s.UserId == userId);
        }

        public async Task AddQuestionUserScore(AnswerUserScore score)
        {
            await _context.QuestionUserScores.AddAsync(score);
        }



        #endregion

    }
}
