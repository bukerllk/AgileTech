﻿// <auto-generated />
using System;
using AgileTech_API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AgileTech_API.Migrations
{
    [DbContext(typeof(ApplicatioDbContext))]
    [Migration("20231005172326_AddDataToClient")]
    partial class AddDataToClient
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AgileTech_API.Models.Client", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("clients");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Created = new DateTime(2023, 10, 5, 13, 23, 26, 465, DateTimeKind.Local).AddTicks(3087),
                            Email = "eduard@gmail.com",
                            Name = "Eduard Name"
                        },
                        new
                        {
                            Id = 2,
                            Created = new DateTime(2023, 10, 5, 13, 23, 26, 465, DateTimeKind.Local).AddTicks(3138),
                            Email = "alis@gmail.com",
                            Name = "Alis Navarrete"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}