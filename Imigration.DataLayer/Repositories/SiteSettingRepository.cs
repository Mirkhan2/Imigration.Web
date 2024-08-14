using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imigration.DataLayer.Context;
using Imigration.Domains.Entities.SiteSetting;
using Imigration.Domains.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Imigration.DataLayer.Repositories
{
    public class SiteSettingRepository : ISiteSettingRepository
    {
        #region Ctor
        private ImigrationDbContext _context;
        public SiteSettingRepository(ImigrationDbContext context)
        {
            _context = context;
        }

        #endregion

        public async Task<EmailSetting> GetDefaultEmail()
        {
            return await _context.EmailSettings.FirstOrDefaultAsync(s => !s.IsDelete && s.IsDefault);
        }
    }
}
