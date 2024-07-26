﻿// <auto-generated />
using System;
using HummingbirdFeeder.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HummingbirdFeeder.Migrations
{
    [DbContext(typeof(FeederDataContext))]
    [Migration("20240726012954_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.20");

            modelBuilder.Entity("HummingbirdFeeder.Data.Feeder", b =>
                {
                    b.Property<int>("FeederId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool?>("ChangeFeeder")
                        .HasColumnType("INTEGER");

                    b.Property<string>("FeederName")
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<int>("LastChangeDate")
                        .HasMaxLength(8)
                        .HasColumnType("INTEGER");

                    b.Property<string>("Zipcode")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("TEXT");

                    b.HasKey("FeederId");

                    b.ToTable("Feeder", (string)null);

                    b.HasData(
                        new
                        {
                            FeederId = 1,
                            ChangeFeeder = true,
                            FeederName = "My Feeder",
                            LastChangeDate = 20240725,
                            Zipcode = "40204"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
