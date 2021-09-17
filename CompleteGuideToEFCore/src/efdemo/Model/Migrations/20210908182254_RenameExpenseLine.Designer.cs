﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Model;

namespace Model.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20210908182254_RenameExpenseLine")]
    partial class RenameExpenseLine
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.9");

            modelBuilder.Entity("Model.ExpenseHeader", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("ExpenseDate")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("ExpenseHeaders");
                });

            modelBuilder.Entity("Model.ExpenseLine", b =>
                {
                    b.Property<int>("ExpenseLineId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("UnitCost")
                        .HasColumnType("decimal(16, 2)");

                    b.HasKey("ExpenseLineId");

                    b.ToTable("ExpenseLines");
                });
#pragma warning restore 612, 618
        }
    }
}