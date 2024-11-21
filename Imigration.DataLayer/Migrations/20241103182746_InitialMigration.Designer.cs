﻿// <auto-generated />
using System;
using Imigration.DataLayer.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Imigration.DataLayer.Migrations
{
    [DbContext(typeof(ImigrationDbContext))]
    [Migration("20241103182746_InitialMigration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Imigration.Domains.Entities.Account.Permission", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Permissions");
                });

            modelBuilder.Entity("Imigration.Domains.Entities.Account.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("Avatar")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<long?>("CityId")
                        .HasColumnType("bigint");

                    b.Property<long?>("CountryId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("EmailActivationCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("GetNewsLetter")
                        .HasColumnType("bit");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("bit");

                    b.Property<bool>("IsBan")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<bool>("IsEmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int?>("Medal")
                        .HasColumnType("int");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<int>("Score")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.HasIndex("CountryId");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            Avatar = "DefaultAvatar.png",
                            CreateDate = new DateTime(2024, 11, 3, 19, 27, 45, 884, DateTimeKind.Local).AddTicks(3684),
                            Email = "mirkhan.shams4@gmail.com",
                            EmailActivationCode = "faf72306107a4dc2aa8994f1174d238d",
                            GetNewsLetter = false,
                            IsAdmin = true,
                            IsBan = false,
                            IsDelete = false,
                            IsEmailConfirmed = true,
                            Password = "96-E7-92-18-96-5E-B7-2C-92-A5-49-DD-5A-33-01-12",
                            Score = 0
                        });
                });

            modelBuilder.Entity("Imigration.Domains.Entities.Account.UserPermission", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<long>("PermissionId")
                        .HasColumnType("bigint");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("PermissionId");

                    b.HasIndex("UserId");

                    b.ToTable("UserPermissions");
                });

            modelBuilder.Entity("Imigration.Domains.Entities.Account.UserQuestionBookmark", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<long>("QuestionId")
                        .HasColumnType("bigint");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("QuestionId");

                    b.HasIndex("UserId");

                    b.ToTable("Bookmarks");
                });

            modelBuilder.Entity("Imigration.Domains.Entities.Location.State", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<long?>("ParentId")
                        .HasColumnType("bigint");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.ToTable("States");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            CreateDate = new DateTime(2024, 11, 3, 19, 27, 45, 884, DateTimeKind.Local).AddTicks(3786),
                            IsDelete = false,
                            Title = "ایران"
                        },
                        new
                        {
                            Id = 4L,
                            CreateDate = new DateTime(2024, 11, 3, 19, 27, 45, 884, DateTimeKind.Local).AddTicks(3795),
                            IsDelete = false,
                            ParentId = 1L,
                            Title = "تبریز"
                        },
                        new
                        {
                            Id = 3L,
                            CreateDate = new DateTime(2024, 11, 3, 19, 27, 45, 884, DateTimeKind.Local).AddTicks(3806),
                            IsDelete = false,
                            ParentId = 1L,
                            Title = "اصفهان"
                        },
                        new
                        {
                            Id = 2L,
                            CreateDate = new DateTime(2024, 11, 3, 19, 27, 45, 884, DateTimeKind.Local).AddTicks(3813),
                            IsDelete = false,
                            ParentId = 1L,
                            Title = "تهران"
                        });
                });

            modelBuilder.Entity("Imigration.Domains.Entities.Questions.Answer", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<bool>("IsTrue")
                        .HasColumnType("bit");

                    b.Property<long>("QuestionId")
                        .HasColumnType("bigint");

                    b.Property<int>("Score")
                        .HasColumnType("int");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("QuestionId");

                    b.HasIndex("UserId");

                    b.ToTable("Answers");
                });

            modelBuilder.Entity("Imigration.Domains.Entities.Questions.AnswerUserScore", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<long>("AnswerId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("AnswerId");

                    b.HasIndex("UserId");

                    b.ToTable("AnswerUserScores");
                });

            modelBuilder.Entity("Imigration.Domains.Entities.Questions.Question", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsChecked")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<long>("QuestionId")
                        .HasColumnType("bigint");

                    b.Property<int>("Score")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.Property<int>("ViewCount")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("Imigration.Domains.Entities.Questions.QuestionUserScore", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<long>("QuestionId")
                        .HasColumnType("bigint");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("QuestionId");

                    b.HasIndex("UserId");

                    b.ToTable("QuestionUserScores");
                });

            modelBuilder.Entity("Imigration.Domains.Entities.Questions.QuestionView", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<long>("QuestionId")
                        .HasColumnType("bigint");

                    b.Property<string>("UserIP")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("QuestionId");

                    b.ToTable("QuestionViews");
                });

            modelBuilder.Entity("Imigration.Domains.Entities.Questions.SelectQuestionTag", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<long>("QuestionId")
                        .HasColumnType("bigint");

                    b.Property<long>("TagId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("QuestionId");

                    b.HasIndex("TagId");

                    b.ToTable("SelectQuestionTags");
                });

            modelBuilder.Entity("Imigration.Domains.Entities.SiteSetting.EmailSetting", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("EnableSSL")
                        .HasColumnType("bit");

                    b.Property<string>("From")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDefault")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Port")
                        .HasColumnType("int");

                    b.Property<string>("SMTP")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("EmailSettings");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            CreateDate = new DateTime(2024, 11, 3, 19, 27, 45, 884, DateTimeKind.Local).AddTicks(3584),
                            DisplayName = "Imigration Email",
                            EnableSSL = true,
                            From = "mirkhan.shams4@gmail.com",
                            IsDefault = true,
                            IsDelete = false,
                            Password = "amanyxlfuwtmdlnk",
                            Port = 587,
                            SMTP = "smtp.gmail.com"
                        });
                });

            modelBuilder.Entity("Imigration.Domains.Entities.Tags.RequestTag", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("RequestTags");
                });

            modelBuilder.Entity("Imigration.Domains.Entities.Tags.Tag", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("UseCount")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Tags");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            CreateDate = new DateTime(2024, 11, 3, 19, 27, 45, 884, DateTimeKind.Local).AddTicks(3828),
                            IsDelete = false,
                            Title = "برنامه نویسی",
                            UseCount = 0
                        },
                        new
                        {
                            Id = 2L,
                            CreateDate = new DateTime(2024, 11, 3, 19, 27, 45, 884, DateTimeKind.Local).AddTicks(3835),
                            IsDelete = false,
                            Title = "طراحی سایت",
                            UseCount = 0
                        });
                });

            modelBuilder.Entity("Imigration.Domains.Entities.Account.User", b =>
                {
                    b.HasOne("Imigration.Domains.Entities.Location.State", "City")
                        .WithMany("UserCities")
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Imigration.Domains.Entities.Location.State", "Country")
                        .WithMany("UserCountries")
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("City");

                    b.Navigation("Country");
                });

            modelBuilder.Entity("Imigration.Domains.Entities.Account.UserPermission", b =>
                {
                    b.HasOne("Imigration.Domains.Entities.Account.Permission", "Permission")
                        .WithMany("UserPermission")
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Imigration.Domains.Entities.Account.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Permission");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Imigration.Domains.Entities.Account.UserQuestionBookmark", b =>
                {
                    b.HasOne("Imigration.Domains.Entities.Questions.Question", "Question")
                        .WithMany("UserQuestionBookmarks")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Imigration.Domains.Entities.Account.User", "User")
                        .WithMany("UserQuestionBookmarks")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Question");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Imigration.Domains.Entities.Location.State", b =>
                {
                    b.HasOne("Imigration.Domains.Entities.Location.State", "Parent")
                        .WithMany()
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("Imigration.Domains.Entities.Questions.Answer", b =>
                {
                    b.HasOne("Imigration.Domains.Entities.Questions.Question", "Question")
                        .WithMany("Answers")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Imigration.Domains.Entities.Account.User", "User")
                        .WithMany("Answers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Question");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Imigration.Domains.Entities.Questions.AnswerUserScore", b =>
                {
                    b.HasOne("Imigration.Domains.Entities.Questions.Answer", "Answer")
                        .WithMany("AnswerUserScores")
                        .HasForeignKey("AnswerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Imigration.Domains.Entities.Account.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Answer");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Imigration.Domains.Entities.Questions.Question", b =>
                {
                    b.HasOne("Imigration.Domains.Entities.Account.User", "User")
                        .WithMany("Questions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Imigration.Domains.Entities.Questions.QuestionUserScore", b =>
                {
                    b.HasOne("Imigration.Domains.Entities.Questions.Question", "Question")
                        .WithMany("QuestionUserScores")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Imigration.Domains.Entities.Account.User", "User")
                        .WithMany("QuestionUserScores")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Question");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Imigration.Domains.Entities.Questions.QuestionView", b =>
                {
                    b.HasOne("Imigration.Domains.Entities.Questions.Question", "Question")
                        .WithMany("QuestionViews")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Question");
                });

            modelBuilder.Entity("Imigration.Domains.Entities.Questions.SelectQuestionTag", b =>
                {
                    b.HasOne("Imigration.Domains.Entities.Questions.Question", "Question")
                        .WithMany("SelectQuestionTags")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Imigration.Domains.Entities.Tags.Tag", "Tag")
                        .WithMany("SelectQuestionTags")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Question");

                    b.Navigation("Tag");
                });

            modelBuilder.Entity("Imigration.Domains.Entities.Tags.RequestTag", b =>
                {
                    b.HasOne("Imigration.Domains.Entities.Account.User", "User")
                        .WithMany("RequestTags")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Imigration.Domains.Entities.Account.Permission", b =>
                {
                    b.Navigation("UserPermission");
                });

            modelBuilder.Entity("Imigration.Domains.Entities.Account.User", b =>
                {
                    b.Navigation("Answers");

                    b.Navigation("QuestionUserScores");

                    b.Navigation("Questions");

                    b.Navigation("RequestTags");

                    b.Navigation("UserQuestionBookmarks");
                });

            modelBuilder.Entity("Imigration.Domains.Entities.Location.State", b =>
                {
                    b.Navigation("UserCities");

                    b.Navigation("UserCountries");
                });

            modelBuilder.Entity("Imigration.Domains.Entities.Questions.Answer", b =>
                {
                    b.Navigation("AnswerUserScores");
                });

            modelBuilder.Entity("Imigration.Domains.Entities.Questions.Question", b =>
                {
                    b.Navigation("Answers");

                    b.Navigation("QuestionUserScores");

                    b.Navigation("QuestionViews");

                    b.Navigation("SelectQuestionTags");

                    b.Navigation("UserQuestionBookmarks");
                });

            modelBuilder.Entity("Imigration.Domains.Entities.Tags.Tag", b =>
                {
                    b.Navigation("SelectQuestionTags");
                });
#pragma warning restore 612, 618
        }
    }
}