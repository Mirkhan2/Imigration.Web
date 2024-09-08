using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imigration.Domains.Entities.Account;
using Imigration.Domains.Entities.Common;

namespace Imigration.Domains.Entities.Questions
{
    public class Answer : BaseEntity
    {
        #region Properties

        [Display(Name = "پاسخ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Content { get; set; }

        public long UserId { get; set; }

        [Display(Name = "امتیاز")]
        public int Score { get; set; } = 0;

        public bool IsTrue { get; set; }

        public long QuestionId { get; set; }

        #endregion

        #region Relations

        public User User { get; set; }

        public Question Question { get; set; }
        public ICollection<AnswerUserScore> AnswerUserScores { get; set; }

        #endregion
    }
}
