﻿// <auto-generated />
using System;
using BlogApps.Data.Concrete.EfCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BlogApps.Migrations
{
    [DbContext(typeof(BlogContext))]
    [Migration("20241215123207_PostRequestUpdate")]
    partial class PostRequestUpdate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("BlogApps.Entity.Categories", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("CategoryId"));

                    b.Property<string>("Text")
                        .HasColumnType("longtext");

                    b.Property<string>("Url")
                        .HasColumnType("longtext");

                    b.HasKey("CategoryId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("BlogApps.Entity.Comment", b =>
                {
                    b.Property<int>("CommentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("CommentId"));

                    b.Property<int>("PostId")
                        .HasColumnType("int");

                    b.Property<DateTime>("PublishedOn")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Text")
                        .HasColumnType("longtext");

                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)");

                    b.HasKey("CommentId");

                    b.HasIndex("PostId");

                    b.HasIndex("UserId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("BlogApps.Entity.LikePosts", b =>
                {
                    b.Property<int>("LikedPostId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("LikedPostId"));

                    b.Property<int>("PostId")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)");

                    b.Property<bool>("kontrol")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("LikedPostId");

                    b.HasIndex("PostId")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("LikePosts");
                });

            modelBuilder.Entity("BlogApps.Entity.Post", b =>
                {
                    b.Property<int>("PostId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("PostId"));

                    b.Property<string>("Content")
                        .HasColumnType("longtext");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<string>("Image")
                        .HasColumnType("longtext");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsBest")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsRequestRequired")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("LikedPostId")
                        .HasColumnType("int");

                    b.Property<bool>("PdfActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<DateTime>("PublishedOn")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("SHA256Hash")
                        .HasColumnType("longtext");

                    b.Property<string>("Title")
                        .HasColumnType("longtext");

                    b.Property<string>("Url")
                        .HasColumnType("longtext");

                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)");

                    b.Property<int>("Vote")
                        .HasColumnType("int");

                    b.HasKey("PostId");

                    b.HasIndex("UserId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("BlogApps.Entity.PostRequest", b =>
                {
                    b.Property<int>("RequestId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("RequestId"));

                    b.Property<bool>("IsAccepted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("OwnerId")
                        .HasColumnType("varchar(255)");

                    b.Property<int>("PostId")
                        .HasColumnType("int");

                    b.Property<DateTime>("RequestedOn")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("RequesterId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)");

                    b.HasKey("RequestId");

                    b.HasIndex("OwnerId");

                    b.HasIndex("PostId");

                    b.HasIndex("RequesterId");

                    b.HasIndex("UserId");

                    b.ToTable("PostRequests");
                });

            modelBuilder.Entity("BlogApps.Entity.Tag", b =>
                {
                    b.Property<int>("TagId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("TagId"));

                    b.Property<string>("Text")
                        .HasColumnType("longtext");

                    b.Property<string>("Url")
                        .HasColumnType("longtext");

                    b.HasKey("TagId");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("BlogApps.Entity.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("AboutMe")
                        .HasColumnType("longtext");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext");

                    b.Property<bool>("Copyright")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("FullName")
                        .HasColumnType("longtext");

                    b.Property<string>("Image")
                        .HasColumnType("longtext");

                    b.Property<string>("Linkedin")
                        .HasColumnType("longtext");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("longtext");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("longtext");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("BlogApps.Entity.UserRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("CategoriesPost", b =>
                {
                    b.Property<int>("categoriesCategoryId")
                        .HasColumnType("int");

                    b.Property<int>("postsPostId")
                        .HasColumnType("int");

                    b.HasKey("categoriesCategoryId", "postsPostId");

                    b.HasIndex("postsPostId");

                    b.ToTable("CategoriesPost");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("longtext");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("RoleId")
                        .HasColumnType("varchar(255)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Value")
                        .HasColumnType("longtext");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("PostTag", b =>
                {
                    b.Property<int>("postsPostId")
                        .HasColumnType("int");

                    b.Property<int>("tagsTagId")
                        .HasColumnType("int");

                    b.HasKey("postsPostId", "tagsTagId");

                    b.HasIndex("tagsTagId");

                    b.ToTable("PostTag");
                });

            modelBuilder.Entity("BlogApps.Entity.Comment", b =>
                {
                    b.HasOne("BlogApps.Entity.Post", "post")
                        .WithMany("comments")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BlogApps.Entity.User", "user")
                        .WithMany("comments")
                        .HasForeignKey("UserId");

                    b.Navigation("post");

                    b.Navigation("user");
                });

            modelBuilder.Entity("BlogApps.Entity.LikePosts", b =>
                {
                    b.HasOne("BlogApps.Entity.Post", "Post")
                        .WithOne("LikePost")
                        .HasForeignKey("BlogApps.Entity.LikePosts", "PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BlogApps.Entity.User", "user")
                        .WithMany("LikePost")
                        .HasForeignKey("UserId");

                    b.Navigation("Post");

                    b.Navigation("user");
                });

            modelBuilder.Entity("BlogApps.Entity.Post", b =>
                {
                    b.HasOne("BlogApps.Entity.User", "User")
                        .WithMany("posts")
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BlogApps.Entity.PostRequest", b =>
                {
                    b.HasOne("BlogApps.Entity.User", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId");

                    b.HasOne("BlogApps.Entity.Post", "Post")
                        .WithMany()
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BlogApps.Entity.User", "Requester")
                        .WithMany()
                        .HasForeignKey("RequesterId");

                    b.HasOne("BlogApps.Entity.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("Owner");

                    b.Navigation("Post");

                    b.Navigation("Requester");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CategoriesPost", b =>
                {
                    b.HasOne("BlogApps.Entity.Categories", null)
                        .WithMany()
                        .HasForeignKey("categoriesCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BlogApps.Entity.Post", null)
                        .WithMany()
                        .HasForeignKey("postsPostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("BlogApps.Entity.UserRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("BlogApps.Entity.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("BlogApps.Entity.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("BlogApps.Entity.UserRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BlogApps.Entity.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("BlogApps.Entity.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PostTag", b =>
                {
                    b.HasOne("BlogApps.Entity.Post", null)
                        .WithMany()
                        .HasForeignKey("postsPostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BlogApps.Entity.Tag", null)
                        .WithMany()
                        .HasForeignKey("tagsTagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BlogApps.Entity.Post", b =>
                {
                    b.Navigation("LikePost");

                    b.Navigation("comments");
                });

            modelBuilder.Entity("BlogApps.Entity.User", b =>
                {
                    b.Navigation("LikePost");

                    b.Navigation("comments");

                    b.Navigation("posts");
                });
#pragma warning restore 612, 618
        }
    }
}
