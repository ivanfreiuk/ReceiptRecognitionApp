﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ReceiptRecognitionApp.Contenxt;

namespace ReceiptRecognitionApp.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20191211195821_AddFieldsToReceipt")]
    partial class AddFieldsToReceipt
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ReceiptRecognitionApp.Entities.Receipt", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Json");

                    b.Property<string>("ReceiptDate");

                    b.Property<int>("ReceiptImageId");

                    b.Property<string>("ReceiptTotal");

                    b.Property<string>("Text");

                    b.HasKey("Id");

                    b.HasIndex("ReceiptImageId")
                        .IsUnique();

                    b.ToTable("Receipts");
                });

            modelBuilder.Entity("ReceiptRecognitionApp.Entities.ReceiptImage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<byte[]>("OriginalImage");

                    b.Property<string>("OriginalImageName");

                    b.Property<byte[]>("ScannedImage");

                    b.Property<string>("ScannedImageName");

                    b.HasKey("Id");

                    b.ToTable("ReceiptImages");
                });

            modelBuilder.Entity("ReceiptRecognitionApp.Entities.Receipt", b =>
                {
                    b.HasOne("ReceiptRecognitionApp.Entities.ReceiptImage", "ReceiptImage")
                        .WithOne("Receipt")
                        .HasForeignKey("ReceiptRecognitionApp.Entities.Receipt", "ReceiptImageId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
