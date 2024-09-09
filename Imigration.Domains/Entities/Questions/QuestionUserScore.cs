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
    public class QuestionUserScore : BaseEntity
    {
        #region Properties

        public long UserId { get; set; }

        public long QuestionId { get; set; }

        public QuestionScoreType Type { get; set; }

        #endregion

        #region Relations

        public User User { get; set; }

        public Question Question { get; set; }

        #endregion

    }
}
