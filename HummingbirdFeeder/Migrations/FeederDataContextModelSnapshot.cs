﻿// <auto-generated />
using HummingbirdFeeder.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HummingbirdFeeder.Migrations
{
    [DbContext(typeof(FeederDataContext))]
    partial class FeederDataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.20");

            modelBuilder.Entity("HummingbirdFeeder.Data.Feeder", b =>
                {
                    b.Property<int>("FeederId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Zipcode")
                        .HasColumnType("INTEGER");

                    b.HasKey("FeederId");

                    b.ToTable("Feeder", (string)null);

                    b.HasData(
                        new
                        {
                            FeederId = 1,
                            Zipcode = 40204
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
