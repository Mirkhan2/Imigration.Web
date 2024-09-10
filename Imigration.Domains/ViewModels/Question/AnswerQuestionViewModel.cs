using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imigration.Domains.ViewModels.Question
{
    public class AnswerQuestionViewModel
    {
        public string Answer { get; set; }
        public long QuestionId { get; set; }
        public long UserId { get; set; }

    }
    public class EditAnswerViewModel
    {
        public string Answer { get; set; }
        public long AnswerId { get; set; }
        public long UserId { get; set; }
        public long QuestionId { get; set; }

    }
}
