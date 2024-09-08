using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imigration.Domains.ViewModels.Common
{
    public class ScoreManagementViewModel
    {
        public int MinRequestsCountForVerifyTag { get; set; }

        public int AddNewQuestionScrore { get; set; }
        public int AddNewAnswerScrore { get; set; }

        public int MinScoreForBronzeMedal { get; set; }
  

        public int MinScoreForSilverMedal { get; set; }

        public int MinScoreForGoldMedal { get; set; }
        public int MinScoreForUpScoreAnswer { get; set; }
        public int MinScoreForDownScoreAnswer { get; set; }

    }
}
