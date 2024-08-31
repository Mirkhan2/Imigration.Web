using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imigration.Domains.Entities.Account;
using Imigration.Domains.Entities.Tags;

namespace Imigration.Domains.Entities.Questions
{
    public class SelectQuestionTag
    {
        #region Properties
        public long Id { get; set; }
        public long QuestionId { get; set; }
        public long TagId { get; set; }
        #endregion
        #region Relations
        public User  User { get; set; }
        
        #endregion
    }
}
