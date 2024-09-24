using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imigration.Domains.ViewModels.Common;

namespace Imigration.Domains.ViewModels.Admin.Tag
{
    public class FilterTagAdminViewModel : Paging<Domains.Entities.Tags.Tag>
    {
        public FilterTagAdminViewModel()
        {
            Status = FilterAdminStatus.All;
        }
        public string? Title { get; set; }
        public FilterAdminStatus Status { get; set; }
    }
    public enum FilterAdminStatus
    {
        All,

        HasDescription,

        NoDescription
    }
}
