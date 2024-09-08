using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imigration.Domains.Entities.Account;
using Imigration.Domains.Entities.Common;
using Imigration.Domains.Enums;

namespace Imigration.Domains.Entities.Questions
{
    public class AnswerUserScore : BaseEntity
    {
        #region properties
        public long UserId { get; set; }
        public long AnswerId { get; set; }
        public AnswerScoreType Type { get; set; }

        #endregion
        #region Relations
        public User User { get; set; }
        public Answer Answer { get; set; }
        #endregion
    }
}
