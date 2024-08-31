using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imigration.Domains.Entities.Common;

namespace Imigration.Domains.Entities.Questions
{
    public class QuestionView : BaseEntity
    {
        #region Properties
        public string UserIP { get; set; }
        public long QuestionId { get; set; }
        #endregion

        #region Relations
        public Question Question { get; set; }
        #endregion
    }
}
