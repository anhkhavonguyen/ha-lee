﻿// <auto-generated />
using System;
using System.ComponentModel.DataAnnotations.Schema;
using Harvey.PIM.Application.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Harvey.PIM.Application.Infrastructure.Migrations
{
    [DbContext(typeof(PimDbContext))]
    [Migration("20181123110418_drop_variant_fk")]
    partial class drop_variant_fk
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Harvey.Domain.AppSetting", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Key");

                    b.Property<string>("Value");

                    b.HasKey("Id");

                    b.ToTable("AppSettings");
                });

            modelBuilder.Entity("Harvey.PIM.Application.FieldFramework.Entities.EntityRef", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<string>("Namespace");

                    b.HasKey("Id");

                    b.ToTable("EntityRefs");
                });

            modelBuilder.Entity("Harvey.PIM.Application.FieldFramework.Entities.Field", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("DefaultValue");

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<int>("Type");

                    b.Property<Guid>("UpdatedBy");

                    b.Property<DateTime>("UpdatedDate");

                    b.HasKey("Id");

                    b.ToTable("Fields");
                });

            modelBuilder.Entity("Harvey.PIM.Application.FieldFramework.Entities.Field_FieldTemplate", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("FieldId");

                    b.Property<Guid>("FieldTemplateId");

                    b.Property<bool>("IsVariantField");

                    b.Property<int>("OrderSection");

                    b.Property<string>("Section");

                    b.HasKey("Id");

                    b.HasIndex("FieldTemplateId");

                    b.HasIndex("FieldId", "FieldTemplateId")
                        .IsUnique();

                    b.ToTable("Field_FieldTemplates");
                });

            modelBuilder.Entity("Harvey.PIM.Application.FieldFramework.Entities.FieldTemplate", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("FieldTemplates");
                });

            modelBuilder.Entity("Harvey.PIM.Application.FieldFramework.Entities.FieldValue", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("BooleanValue");

                    b.Property<Guid>("EntityId");

                    b.Property<string>("EntityReferenceValue");

                    b.Property<Guid>("FieldId");

                    b.Property<decimal>("NumericValue");

                    b.Property<string>("PredefinedListValue");

                    b.Property<string>("RichTextValue");

                    b.Property<string>("TagsValue");

                    b.Property<string>("TextValue");

                    b.HasKey("Id");

                    b.HasIndex("FieldId");

                    b.ToTable("FieldValues");
                });

            modelBuilder.Entity("Harvey.PIM.Application.Infrastructure.Domain.Assortment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<Guid>("UpdatedBy");

                    b.Property<DateTime>("UpdatedDate");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Assortments");
                });

            modelBuilder.Entity("Harvey.PIM.Application.Infrastructure.Domain.Brand", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<Guid>("UpdatedBy");

                    b.Property<DateTime>("UpdatedDate");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Brands");
                });

            modelBuilder.Entity("Harvey.PIM.Application.Infrastructure.Domain.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<Guid>("UpdatedBy");

                    b.Property<DateTime>("UpdatedDate");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Harvey.PIM.Application.Infrastructure.Domain.Channel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<string>("ServerInformation");

                    b.Property<Guid>("UpdatedBy");

                    b.Property<DateTime>("UpdatedDate");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("ServerInformation")
                        .IsUnique();

                    b.ToTable("Channels");
                });

            modelBuilder.Entity("Harvey.PIM.Application.Infrastructure.Domain.Location", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<Guid>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Name");

                    b.Property<int>("Type");

                    b.Property<Guid>("UpdatedBy");

                    b.Property<DateTime>("UpdatedDate");

                    b.HasKey("Id");

                    b.HasIndex("Name", "Type")
                        .IsUnique();

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("Harvey.PIM.Application.Infrastructure.Domain.Price", b =>
                {
                    b.Property<Guid>("Id")
                        .HasAnnotation("DatabaseGenerated", DatabaseGeneratedOption.None);

                    b.Property<Guid>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<float>("ListPrice");

                    b.Property<float>("MemberPrice");

                    b.Property<float>("StaffPrice");

                    b.Property<Guid>("UpdatedBy");

                    b.Property<DateTime>("UpdatedDate");

                    b.HasKey("Id");

                    b.ToTable("Price");
                });

            modelBuilder.Entity("Harvey.PIM.Application.Infrastructure.Domain.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .HasAnnotation("DatabaseGenerated", DatabaseGeneratedOption.None);

                    b.Property<Guid>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Description");

                    b.Property<Guid>("FieldTemplateId");

                    b.Property<string>("Name");

                    b.Property<Guid>("UpdatedBy");

                    b.Property<DateTime>("UpdatedDate");

                    b.HasKey("Id");

                    b.HasIndex("FieldTemplateId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Harvey.PIM.Application.Infrastructure.Domain.Variant", b =>
                {
                    b.Property<Guid>("Id")
                        .HasAnnotation("DatabaseGenerated", DatabaseGeneratedOption.None);

                    b.Property<Guid>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<Guid>("PriceId");

                    b.Property<Guid>("ProductId");

                    b.Property<Guid>("UpdatedBy");

                    b.Property<DateTime>("UpdatedDate");

                    b.HasKey("Id");

                    b.ToTable("Variants");
                });

            modelBuilder.Entity("Harvey.PIM.Application.FieldFramework.Entities.Field_FieldTemplate", b =>
                {
                    b.HasOne("Harvey.PIM.Application.FieldFramework.Entities.Field", "Field")
                        .WithMany()
                        .HasForeignKey("FieldId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Harvey.PIM.Application.FieldFramework.Entities.FieldTemplate", "FieldTemplate")
                        .WithMany("Field_FieldTemplates")
                        .HasForeignKey("FieldTemplateId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Harvey.PIM.Application.FieldFramework.Entities.FieldValue", b =>
                {
                    b.HasOne("Harvey.PIM.Application.FieldFramework.Entities.Field", "Field")
                        .WithMany("FieldValues")
                        .HasForeignKey("FieldId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Harvey.PIM.Application.Infrastructure.Domain.Product", b =>
                {
                    b.HasOne("Harvey.PIM.Application.FieldFramework.Entities.FieldTemplate", "FieldTemplate")
                        .WithMany()
                        .HasForeignKey("FieldTemplateId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
