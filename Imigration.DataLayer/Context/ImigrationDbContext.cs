using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imigration.Domains.Entities.Account;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Imigration.DataLayer.Context
{
    public class ImigrationDbContext : DbContext
    {
        #region Ctor

        public ImigrationDbContext(DbContextOptions<ImigrationDbContext> options) : base(options)
        {

        }
        #endregion

        #region DbSet
        public DbSet<User> Users { get; set; }

        #endregion
    }
}
