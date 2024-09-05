using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imigration.Domains.Entities.Tags;
using Imigration.Domains.ViewModels.Common;

namespace Imigration.Domains.ViewModels.Question
{
    public class FilterTagViewModel : Paging<Tag>
    {

        public string? Title { get; set; }
        public FilterTagEnum Sort { get; set; }
    }
    public enum FilterTagEnum
    {
        [Display(Name = "تاریخ ثبت نزولی")] NewToOld,
        [Display(Name = "تاریخ ثبت صعودی")] OldToNew,
        [Display(Name = "Use نزولی")] UserCountHighToLow,
        [Display(Name = "Use صعودی")] UseCountLOwToHigh
    }
}
