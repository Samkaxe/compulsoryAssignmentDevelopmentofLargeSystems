﻿// <auto-generated />
using System;
using HistoryService.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HistoryService.Migrations
{
    [DbContext(typeof(HistoryContext))]
    [Migration("20240314121929_initial-create")]
    partial class initialcreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.16");

            modelBuilder.Entity("HistoryService.Domain.OperationEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Operand1")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Operand2")
                        .HasColumnType("TEXT");

                    b.Property<string>("OperationType")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Result")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("OperationEntries");
                });
#pragma warning restore 612, 618
        }
    }
}
