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
    [Migration("20210910161603_UserToExpenseHeader")]
    partial class UserToExpenseHeader
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

                    b.Property<int>("ApproverId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("ExpenseDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("RequesterId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ApproverId");

                    b.HasIndex("RequesterId");

                    b.ToTable("ExpenseHeaders");
                });

            modelBuilder.Entity("Model.ExpenseLine", b =>
                {
                    b.Property<int>("ExpenseLineId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<int>("ExpenseHeaderId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("UnitCost")
                        .HasColumnType("decimal(16, 2)");

                    b.HasKey("ExpenseLineId");

                    b.HasIndex("ExpenseHeaderId");

                    b.ToTable("ExpenseLines");
                });

            modelBuilder.Entity("Model.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("FirstName")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Model.ExpenseHeader", b =>
                {
                    b.HasOne("Model.User", "Approver")
                        .WithMany("ApproverExpenseHeaders")
                        .HasForeignKey("ApproverId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Model.User", "Requester")
                        .WithMany("RequesterExpenseHeaders")
                        .HasForeignKey("RequesterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Approver");

                    b.Navigation("Requester");
                });

            modelBuilder.Entity("Model.ExpenseLine", b =>
                {
                    b.HasOne("Model.ExpenseHeader", "ExpenseHeader")
                        .WithMany("ExpenseLines")
                        .HasForeignKey("ExpenseHeaderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ExpenseHeader");
                });

            modelBuilder.Entity("Model.ExpenseHeader", b =>
                {
                    b.Navigation("ExpenseLines");
                });

            modelBuilder.Entity("Model.User", b =>
                {
                    b.Navigation("ApproverExpenseHeaders");

                    b.Navigation("RequesterExpenseHeaders");
                });
#pragma warning restore 612, 618
        }
    }
}
