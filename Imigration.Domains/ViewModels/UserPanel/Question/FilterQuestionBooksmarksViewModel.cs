using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imigration.Domains.ViewModels.Common;

namespace Imigration.Domains.ViewModels.UserPanel.Question
{
    public class FilterQuestionBookmarksViewModel : Paging<Domains.Entities.Questions.Question>
    {
        public long UserId { get; set; }
    }
}
