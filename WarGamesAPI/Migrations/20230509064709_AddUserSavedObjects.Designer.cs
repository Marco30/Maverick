﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WarGamesAPI.Data;

#nullable disable

namespace WarGamesAPI.Migrations
{
    [DbContext(typeof(WarGamesContext))]
    [Migration("20230509064709_AddUserSavedObjects")]
    partial class AddUserSavedObjects
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("WarGamesAPI.Model.Address", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("Attention")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CareOf")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Municipality")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Street")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ZipCode")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("Address");

                    b.HasData(
                        new
                        {
                            UserId = 5,
                            City = "Stockholm",
                            Country = "Sweden",
                            Street = "Röntgenvägen 5 lgh 1410",
                            ZipCode = "14152"
                        });
                });

            modelBuilder.Entity("WarGamesAPI.Model.Answer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("ConversationId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int?>("QuestionId")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ConversationId");

                    b.HasIndex("QuestionId");

                    b.ToTable("Answer");
                });

            modelBuilder.Entity("WarGamesAPI.Model.Conversation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Conversation");
                });

            modelBuilder.Entity("WarGamesAPI.Model.Question", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ConversationId")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ConversationId");

                    b.HasIndex("UserId");

                    b.ToTable("Question");
                });

            modelBuilder.Entity("WarGamesAPI.Model.SavedAnswer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int?>("SavedConversationId")
                        .HasColumnType("int");

                    b.Property<int?>("SavedQuestionId")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("SavedConversationId");

                    b.HasIndex("SavedQuestionId");

                    b.ToTable("SavedAnswer");
                });

            modelBuilder.Entity("WarGamesAPI.Model.SavedConversation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("SavedConversation");
                });

            modelBuilder.Entity("WarGamesAPI.Model.SavedQuestion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("SavedConversationId")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SavedConversationId");

                    b.HasIndex("UserId");

                    b.ToTable("SavedQuestion");
                });

            modelBuilder.Entity("WarGamesAPI.Model.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("AgreeMarketing")
                        .HasColumnType("bit");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Gender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MobilePhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProfileImage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SocialSecurityNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("SubscribeToEmailNotification")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("User");

                    b.HasData(
                        new
                        {
                            Id = 5,
                            AgreeMarketing = true,
                            Email = "khaled@khaled.se",
                            FirstName = "Khaled",
                            FullName = "Khaled Abo",
                            Gender = "Man",
                            LastName = "Abo",
                            Password = "123456",
                            SocialSecurityNumber = "198507119595",
                            SubscribeToEmailNotification = true
                        });
                });

            modelBuilder.Entity("WarGamesAPI.Model.Address", b =>
                {
                    b.HasOne("WarGamesAPI.Model.User", "User")
                        .WithOne("Address")
                        .HasForeignKey("WarGamesAPI.Model.Address", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("WarGamesAPI.Model.Answer", b =>
                {
                    b.HasOne("WarGamesAPI.Model.Conversation", "Conversation")
                        .WithMany("Answers")
                        .HasForeignKey("ConversationId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("WarGamesAPI.Model.Question", "Question")
                        .WithMany("Answers")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("Conversation");

                    b.Navigation("Question");
                });

            modelBuilder.Entity("WarGamesAPI.Model.Conversation", b =>
                {
                    b.HasOne("WarGamesAPI.Model.User", "User")
                        .WithMany("Conversations")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("WarGamesAPI.Model.Question", b =>
                {
                    b.HasOne("WarGamesAPI.Model.Conversation", "Conversation")
                        .WithMany("Questions")
                        .HasForeignKey("ConversationId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("WarGamesAPI.Model.User", "User")
                        .WithMany("Questions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Conversation");

                    b.Navigation("User");
                });

            modelBuilder.Entity("WarGamesAPI.Model.SavedAnswer", b =>
                {
                    b.HasOne("WarGamesAPI.Model.SavedConversation", "SavedConversation")
                        .WithMany("SavedAnswers")
                        .HasForeignKey("SavedConversationId");

                    b.HasOne("WarGamesAPI.Model.SavedQuestion", "SavedQuestion")
                        .WithMany("SavedAnswers")
                        .HasForeignKey("SavedQuestionId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("SavedConversation");

                    b.Navigation("SavedQuestion");
                });

            modelBuilder.Entity("WarGamesAPI.Model.SavedConversation", b =>
                {
                    b.HasOne("WarGamesAPI.Model.User", "User")
                        .WithMany("SavedConversations")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("WarGamesAPI.Model.SavedQuestion", b =>
                {
                    b.HasOne("WarGamesAPI.Model.SavedConversation", "SavedConversation")
                        .WithMany("SavedQuestions")
                        .HasForeignKey("SavedConversationId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("WarGamesAPI.Model.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SavedConversation");

                    b.Navigation("User");
                });

            modelBuilder.Entity("WarGamesAPI.Model.Conversation", b =>
                {
                    b.Navigation("Answers");

                    b.Navigation("Questions");
                });

            modelBuilder.Entity("WarGamesAPI.Model.Question", b =>
                {
                    b.Navigation("Answers");
                });

            modelBuilder.Entity("WarGamesAPI.Model.SavedConversation", b =>
                {
                    b.Navigation("SavedAnswers");

                    b.Navigation("SavedQuestions");
                });

            modelBuilder.Entity("WarGamesAPI.Model.SavedQuestion", b =>
                {
                    b.Navigation("SavedAnswers");
                });

            modelBuilder.Entity("WarGamesAPI.Model.User", b =>
                {
                    b.Navigation("Address");

                    b.Navigation("Conversations");

                    b.Navigation("Questions");

                    b.Navigation("SavedConversations");
                });
#pragma warning restore 612, 618
        }
    }
}
