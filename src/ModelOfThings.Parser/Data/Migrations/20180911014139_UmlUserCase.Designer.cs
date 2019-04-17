﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ModelOfThings.Parser.Data;

namespace ModelOfThings.Parser.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20180911014139_UmlUserCase")]
    partial class UmlUserCase
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128);

                    b.Property<string>("Name")
                        .HasMaxLength(128);

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("ModelOfThings.Parser.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("Name");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("ModelOfThings.Parser.Models.CloudProvider", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AccessKey")
                        .IsRequired();

                    b.Property<string>("AccountId")
                        .IsRequired();

                    b.Property<string>("MddApplicationId");

                    b.Property<string>("Provider")
                        .IsRequired();

                    b.Property<string>("SecretAccessKey")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("MddApplicationId");

                    b.ToTable("CloudProviders");
                });

            modelBuilder.Entity("ModelOfThings.Parser.Models.MddApplication", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ApplicationUserId");

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationUserId");

                    b.ToTable("MddApplications");
                });

            modelBuilder.Entity("ModelOfThings.Parser.Models.MddComponent", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Configurable");

                    b.Property<string>("Connections");

                    b.Property<string>("Description");

                    b.Property<string>("MddApplicationId");

                    b.Property<string>("Name");

                    b.Property<string>("ParentId");

                    b.Property<int>("PositionX");

                    b.Property<int>("PositionY");

                    b.Property<string>("Stereotype");

                    b.Property<string>("Type");

                    b.Property<string>("UmlElementId");

                    b.Property<string>("UmlUseCaseId");

                    b.HasKey("Id");

                    b.HasIndex("MddApplicationId");

                    b.HasIndex("UmlUseCaseId");

                    b.ToTable("MddComponents");
                });

            modelBuilder.Entity("ModelOfThings.Parser.Models.MddPropertie", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("MddComponentId");

                    b.Property<string>("Name");

                    b.Property<bool>("Required");

                    b.Property<string>("Type");

                    b.Property<string>("Value");

                    b.HasKey("Id");

                    b.HasIndex("MddComponentId");

                    b.ToTable("MddProperties");
                });

            modelBuilder.Entity("ModelOfThings.Parser.Models.UmlAssociation", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("MemberEnd");

                    b.Property<string>("MemberOwned");

                    b.Property<string>("Type");

                    b.Property<string>("UmlUseCaseId");

                    b.HasKey("Id");

                    b.HasIndex("UmlUseCaseId");

                    b.ToTable("UmlAssociation");
                });

            modelBuilder.Entity("ModelOfThings.Parser.Models.UmlModel", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ApplicationUserId");

                    b.Property<DateTime>("Date");

                    b.Property<string>("MddApplicationId");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationUserId");

                    b.HasIndex("MddApplicationId");

                    b.ToTable("UmlModels");
                });

            modelBuilder.Entity("ModelOfThings.Parser.Models.UmlUseCase", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("MddApplicationId");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("MddApplicationId");

                    b.ToTable("UmlUseCases");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("ModelOfThings.Parser.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("ModelOfThings.Parser.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ModelOfThings.Parser.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("ModelOfThings.Parser.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ModelOfThings.Parser.Models.CloudProvider", b =>
                {
                    b.HasOne("ModelOfThings.Parser.Models.MddApplication", "MddApplication")
                        .WithMany("CloudProviders")
                        .HasForeignKey("MddApplicationId");
                });

            modelBuilder.Entity("ModelOfThings.Parser.Models.MddApplication", b =>
                {
                    b.HasOne("ModelOfThings.Parser.Models.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("ApplicationUserId");
                });

            modelBuilder.Entity("ModelOfThings.Parser.Models.MddComponent", b =>
                {
                    b.HasOne("ModelOfThings.Parser.Models.MddApplication", "MddApplication")
                        .WithMany()
                        .HasForeignKey("MddApplicationId");

                    b.HasOne("ModelOfThings.Parser.Models.UmlUseCase", "UmlUseCase")
                        .WithMany("MddComponents")
                        .HasForeignKey("UmlUseCaseId");
                });

            modelBuilder.Entity("ModelOfThings.Parser.Models.MddPropertie", b =>
                {
                    b.HasOne("ModelOfThings.Parser.Models.MddComponent", "MddComponent")
                        .WithMany("MddProperties")
                        .HasForeignKey("MddComponentId");
                });

            modelBuilder.Entity("ModelOfThings.Parser.Models.UmlAssociation", b =>
                {
                    b.HasOne("ModelOfThings.Parser.Models.UmlUseCase")
                        .WithMany("Associations")
                        .HasForeignKey("UmlUseCaseId");
                });

            modelBuilder.Entity("ModelOfThings.Parser.Models.UmlModel", b =>
                {
                    b.HasOne("ModelOfThings.Parser.Models.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("ApplicationUserId");

                    b.HasOne("ModelOfThings.Parser.Models.MddApplication", "MddApplication")
                        .WithMany("UmlModels")
                        .HasForeignKey("MddApplicationId");
                });

            modelBuilder.Entity("ModelOfThings.Parser.Models.UmlUseCase", b =>
                {
                    b.HasOne("ModelOfThings.Parser.Models.MddApplication")
                        .WithMany("UmlUseCases")
                        .HasForeignKey("MddApplicationId");
                });
#pragma warning restore 612, 618
        }
    }
}
