﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imigration.Domains.Entities.Account;
using Imigration.Domains.Entities.Location;
using Imigration.Domains.Entities.Questions;
using Imigration.Domains.Entities.SiteSetting;
using Imigration.Domains.Entities.Tags;
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

        public DbSet<State> States { get; set; }
        public DbSet<UserQuestionBookmark> Bookmarks { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionView> QuestionViews { get; set; }
        public DbSet<SelectQuestionTag> SelectQuestionTags { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<RequestTag> RequestTags { get; set; }
        public DbSet<QuestionUserScore> QuestionUserScores { get; set; }
        public DbSet<AnswerUserScore> AnswerUserScores { get; set; }
        public DbSet<Permission> Permissions { get; set; }

        public DbSet<UserPermission> UserPermissions { get; set; }


        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relation in modelBuilder.Model.GetEntityTypes().SelectMany(s => s.GetForeignKeys()))
            {
                relation.DeleteBehavior = DeleteBehavior.Restrict;
            }

            #region Seed Data

            modelBuilder.Entity<EmailSetting>().HasData(new EmailSetting()
            {
                Id = 1,
                DisplayName = "Imigration Email",
                From = "mirkhan.shams4@gmail.com",
                Password = "amanyxlfuwtmdlnk",
                SMTP = "smtp.gmail.com",
                Port = 587,
                EnableSSL = true,
                IsDefault = true,
                IsDelete = false,
                CreateDate = DateTime.Now,
            });

            modelBuilder.Entity<User>().HasData(new User()
            {
                Email = "mirkhan.shams4@gmail.com",
                //hashh pass dATA BAse
                Password = "96-E7-92-18-96-5E-B7-2C-92-A5-49-DD-5A-33-01-12", // 111111
                IsAdmin = true,
                Avatar = "DefaultAvatar.png",
                //Crea = DateTime.Now
                EmailActivationCode = Guid.NewGuid().ToString("N"),
                IsEmailConfirmed = true,
                Id = 1
            });

            modelBuilder.Entity<State>().HasData(new State()
            {
                Id = 1,
                CreateDate = DateTime.Now,
                ParentId = null,
                Title = "ایران"
            });

            modelBuilder.Entity<State>().HasData(new State()
            {
                Id = 4,
                CreateDate = DateTime.Now,
                ParentId = 1,
                Title = "تبریز"
            });

            modelBuilder.Entity<State>().HasData(new State()
            {
                Id = 3,
                CreateDate = DateTime.Now,
                ParentId = 1,
                Title = "اصفهان"
            });

            modelBuilder.Entity<State>().HasData(new State()
            {
                Id = 2,
                CreateDate = DateTime.Now,
                ParentId = 1,
                Title = "تهران"
            });

            modelBuilder.Entity<Tag>().HasData(new Tag()
            {
                Id = 1,
                CreateDate = DateTime.Now,
                Title = "برنامه نویسی"
            });

            modelBuilder.Entity<Tag>().HasData(new Tag()
            {
                Id = 2,
                CreateDate = DateTime.Now,
                Title = "طراحی سایت"
            });

            #endregion

            base.OnModelCreating(modelBuilder);
        }
    }
}
