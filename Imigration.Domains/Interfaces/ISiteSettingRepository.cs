using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imigration.Domains.Entities.SiteSetting;

namespace Imigration.Domains.Interfaces
{
    public interface ISiteSettingRepository
    {
        Task<EmailSetting> GetDefaultEmail();
    }
}
