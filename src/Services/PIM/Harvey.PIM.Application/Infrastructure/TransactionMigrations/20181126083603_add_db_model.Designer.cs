﻿// <auto-generated />
using System;
using Harvey.PIM.Application.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Harvey.PIM.Application.Infrastructure.TransactionMigrations
{
    [DbContext(typeof(TransactionDbContext))]
    [Migration("20181126083603_add_db_model")]
    partial class add_db_model
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Harvey.PIM.Application.Infrastructure.Domain.GIWDocument", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("GIWDocuments");
                });

            modelBuilder.Entity("Harvey.PIM.Application.Infrastructure.Domain.GIWDocumentItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("GIWDocumentId");

                    b.Property<int>("Quantity");

                    b.Property<Guid>("StockTypeId");

                    b.Property<Guid>("VariantId");

                    b.HasKey("Id");

                    b.HasIndex("GIWDocumentId");

                    b.HasIndex("StockTypeId");

                    b.HasIndex("VariantId");

                    b.ToTable("GIWDocumentItems");
                });

            modelBuilder.Entity("Harvey.PIM.Application.Infrastructure.Domain.InventoryTransaction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("FromLocationId");

                    b.Property<Guid>("GIWDocumentId");

                    b.Property<Guid>("ToLocationId");

                    b.Property<Guid>("TransactionTypeId");

                    b.HasKey("Id");

                    b.HasIndex("FromLocationId");

                    b.HasIndex("GIWDocumentId");

                    b.HasIndex("ToLocationId");

                    b.HasIndex("TransactionTypeId");

                    b.ToTable("InventoryTransactions");
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

                    b.ToTable("Location");
                });

            modelBuilder.Entity("Harvey.PIM.Application.Infrastructure.Domain.StockTransaction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Balance");

                    b.Property<Guid>("FromLocationId");

                    b.Property<int>("Quantity");

                    b.Property<Guid>("StockTypeId");

                    b.Property<Guid>("ToLocationId");

                    b.Property<Guid>("TransactionTypeId");

                    b.Property<Guid>("VariantId");

                    b.HasKey("Id");

                    b.HasIndex("FromLocationId");

                    b.HasIndex("StockTypeId");

                    b.HasIndex("ToLocationId");

                    b.ToTable("StockTransactions");
                });

            modelBuilder.Entity("Harvey.PIM.Application.Infrastructure.Domain.StockType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code");

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.ToTable("StockTypes");
                });

            modelBuilder.Entity("Harvey.PIM.Application.Infrastructure.Domain.TransactionType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code");

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.ToTable("TransactionTypes");
                });

            modelBuilder.Entity("Harvey.PIM.Application.Infrastructure.Domain.Variant", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<Guid>("PriceId");

                    b.Property<Guid>("ProductId");

                    b.Property<Guid>("UpdatedBy");

                    b.Property<DateTime>("UpdatedDate");

                    b.HasKey("Id");

                    b.ToTable("Variant");
                });

            modelBuilder.Entity("Harvey.PIM.Application.Infrastructure.Domain.GIWDocumentItem", b =>
                {
                    b.HasOne("Harvey.PIM.Application.Infrastructure.Domain.GIWDocument", "GIWDocument")
                        .WithMany()
                        .HasForeignKey("GIWDocumentId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Harvey.PIM.Application.Infrastructure.Domain.StockType", "StockType")
                        .WithMany()
                        .HasForeignKey("StockTypeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Harvey.PIM.Application.Infrastructure.Domain.Variant")
                        .WithMany()
                        .HasForeignKey("VariantId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Harvey.PIM.Application.Infrastructure.Domain.InventoryTransaction", b =>
                {
                    b.HasOne("Harvey.PIM.Application.Infrastructure.Domain.Location")
                        .WithMany()
                        .HasForeignKey("FromLocationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Harvey.PIM.Application.Infrastructure.Domain.GIWDocument")
                        .WithMany()
                        .HasForeignKey("GIWDocumentId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Harvey.PIM.Application.Infrastructure.Domain.Location")
                        .WithMany()
                        .HasForeignKey("ToLocationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Harvey.PIM.Application.Infrastructure.Domain.TransactionType")
                        .WithMany()
                        .HasForeignKey("TransactionTypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Harvey.PIM.Application.Infrastructure.Domain.StockTransaction", b =>
                {
                    b.HasOne("Harvey.PIM.Application.Infrastructure.Domain.Location")
                        .WithMany()
                        .HasForeignKey("FromLocationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Harvey.PIM.Application.Infrastructure.Domain.StockType")
                        .WithMany()
                        .HasForeignKey("StockTypeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Harvey.PIM.Application.Infrastructure.Domain.Location")
                        .WithMany()
                        .HasForeignKey("ToLocationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
