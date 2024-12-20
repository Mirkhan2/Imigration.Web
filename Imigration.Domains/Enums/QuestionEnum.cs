﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imigration.Domains.Enums
{
    public enum  QuestionScoreType
    {
        [Display(Name = "مثبت")] Plus,

        [Display(Name = "منفی")] Minus,
    }
    public enum AnswerScoreType
    {
        [Display(Name = "مثبت")] Plus,

        [Display(Name = "منفی")] Minus,
    }
    public enum CreateScoreForAnswerResult
    {
        Error,

        NOtEnumScoreForDown,

        NOtEnumScoreForUp,

        UserCreateScoreBefore,


        Success
    }
}
