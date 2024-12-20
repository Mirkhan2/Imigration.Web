﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [Key]
        public long Id { get; set; }

        public long QuestionId { get; set; }

        public long TagId { get; set; }

        #endregion

        #region Relations

        public Question Question { get; set; }

        public Tag Tag { get; set; }

        #endregion
    }
}
