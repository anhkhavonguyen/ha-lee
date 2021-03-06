﻿// <auto-generated />
using System;
using Harvey.Notification.Api;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Harvey.Notification.Api.Migrations
{
    [DbContext(typeof(HarveyNotificationDbContext))]
    [Migration("20180702043423_InitialNotificationDbMigration")]
    partial class InitialNotificationDbMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.1.0-rtm-30799")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Harvey.Notification.Application.Entities.ErrorLogEntry", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Caption");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Detail");

                    b.Property<int>("ErrorLogTypeId");

                    b.Property<string>("Source");

                    b.HasKey("Id");

                    b.HasIndex("ErrorLogTypeId");

                    b.ToTable("ErrorLogEntries");
                });

            modelBuilder.Entity("Harvey.Notification.Application.Entities.ErrorLogType", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("TypeName");

                    b.Property<string>("UpdatedBy");

                    b.Property<DateTime>("UpdatedDate");

                    b.HasKey("Id");

                    b.ToTable("ErrorLogTypes");
                });

            modelBuilder.Entity("Harvey.Notification.Application.Entities.Notification", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Content");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<int>("NotificationTypeId");

                    b.Property<string>("Receivers");

                    b.Property<int>("Status");

                    b.Property<int>("TemplateId");

                    b.Property<string>("UpdatedBy");

                    b.Property<DateTime>("UpdatedDate");

                    b.HasKey("Id");

                    b.HasIndex("NotificationTypeId");

                    b.HasIndex("TemplateId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("Harvey.Notification.Application.Entities.NotificationStatus", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("DisplayName");

                    b.HasKey("Id");

                    b.ToTable("NotificationStatus");
                });

            modelBuilder.Entity("Harvey.Notification.Application.Entities.NotificationType", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("TypeName");

                    b.HasKey("Id");

                    b.ToTable("NotificationTypes");
                });

            modelBuilder.Entity("Harvey.Notification.Application.Entities.Template", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Content");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("DisplayName");

                    b.Property<int>("NotificationTypeId");

                    b.Property<string>("TemplateKey");

                    b.Property<string>("UpdatedBy");

                    b.Property<DateTime>("UpdatedDate");

                    b.HasKey("Id");

                    b.HasIndex("NotificationTypeId");

                    b.ToTable("Templates");
                });

            modelBuilder.Entity("Harvey.Notification.Application.Entities.ErrorLogEntry", b =>
                {
                    b.HasOne("Harvey.Notification.Application.Entities.ErrorLogType", "ErrorLogType")
                        .WithMany("ErrorLogEntries")
                        .HasForeignKey("ErrorLogTypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Harvey.Notification.Application.Entities.Notification", b =>
                {
                    b.HasOne("Harvey.Notification.Application.Entities.NotificationType", "NotificationType")
                        .WithMany("Notifications")
                        .HasForeignKey("NotificationTypeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Harvey.Notification.Application.Entities.Template", "Template")
                        .WithMany()
                        .HasForeignKey("TemplateId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Harvey.Notification.Application.Entities.Template", b =>
                {
                    b.HasOne("Harvey.Notification.Application.Entities.NotificationType", "NotificationType")
                        .WithMany("Templates")
                        .HasForeignKey("NotificationTypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
