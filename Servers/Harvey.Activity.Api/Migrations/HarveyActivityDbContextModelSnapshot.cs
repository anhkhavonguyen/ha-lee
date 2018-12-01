﻿// <auto-generated />
using System;
using Harvey.Activity.Api;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Harvey.Activity.Api.Migrations
{
    [DbContext(typeof(HarveyActivityDbContext))]
    partial class HarveyActivityDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.1.3-rtm-32065")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Harvey.Activity.Application.Entities.ActionActivity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ActionAreaId");

                    b.Property<string>("ActionTypeId");

                    b.Property<string>("Comment");

                    b.Property<string>("CreatedBy");

                    b.Property<string>("CreatedByName");

                    b.Property<DateTime?>("CreatedDate");

                    b.Property<string>("Description");

                    b.Property<string>("UpdatedBy");

                    b.Property<DateTime?>("UpdatedDate");

                    b.Property<string>("Value");

                    b.HasKey("Id");

                    b.HasIndex("ActionAreaId");

                    b.HasIndex("ActionTypeId");

                    b.ToTable("Activities");
                });

            modelBuilder.Entity("Harvey.Activity.Application.Entities.ActionType", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ActionActivityId");

                    b.Property<string>("CreatedBy");

                    b.Property<string>("CreatedByName");

                    b.Property<DateTime?>("CreatedDate");

                    b.Property<string>("Name");

                    b.Property<string>("UpdatedBy");

                    b.Property<DateTime?>("UpdatedDate");

                    b.HasKey("Id");

                    b.ToTable("ActionTypies");
                });

            modelBuilder.Entity("Harvey.Activity.Application.Entities.AreaActivity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ActionActivityId");

                    b.Property<string>("AreaPath");

                    b.Property<string>("CreatedBy");

                    b.Property<string>("CreatedByName");

                    b.Property<DateTime?>("CreatedDate");

                    b.Property<string>("Description");

                    b.Property<string>("UpdatedBy");

                    b.Property<DateTime?>("UpdatedDate");

                    b.HasKey("Id");

                    b.ToTable("AreaActivities");
                });

            modelBuilder.Entity("Harvey.Activity.Application.Entities.ErrorLogEntry", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Caption");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Detail");

                    b.Property<int>("ErrorSourceId");

                    b.Property<int>("Source");

                    b.HasKey("Id");

                    b.HasIndex("ErrorSourceId");

                    b.ToTable("ErrorLogEntries");
                });

            modelBuilder.Entity("Harvey.Activity.Application.Entities.ErrorLogSource", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("SourceName");

                    b.HasKey("Id");

                    b.ToTable("ErrorLogSources");
                });

            modelBuilder.Entity("Harvey.Activity.Application.Entities.ActionActivity", b =>
                {
                    b.HasOne("Harvey.Activity.Application.Entities.AreaActivity", "AreaActivity")
                        .WithMany("ActionActivities")
                        .HasForeignKey("ActionAreaId");

                    b.HasOne("Harvey.Activity.Application.Entities.ActionType", "ActionType")
                        .WithMany("ActionActivities")
                        .HasForeignKey("ActionTypeId");
                });

            modelBuilder.Entity("Harvey.Activity.Application.Entities.ErrorLogEntry", b =>
                {
                    b.HasOne("Harvey.Activity.Application.Entities.ErrorLogSource", "ErrorLogSource")
                        .WithMany("ErrorLogEntries")
                        .HasForeignKey("ErrorSourceId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
