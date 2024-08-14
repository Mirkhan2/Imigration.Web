using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imigration.Domains.Entities.Account;
using Imigration.Domains.Entities.SiteSetting;
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
        public DbSet<EmailSetting> EmailSettings { get; set; }


        #endregion
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Seed Data

            var date = DateTime.MinValue;

            modelBuilder.Entity<EmailSetting>().HasData(new EmailSetting()
            {
                CreateDate = date,
                DisplayName = "Imigration",
                EnableSSL = true,
                From="Imigration@gmail.com",
                Id = 1,
                IsDefault = true,
                IsDelete = false,
                Password =" strong@password",
                Port= 587,
                SMTP = "smtp.gmail.com"
            });
            #endregion

            base.OnModelCreating(modelBuilder);
        }
    }
}
