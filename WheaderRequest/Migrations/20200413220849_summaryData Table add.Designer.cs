﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WheaderRequest;

namespace WheaderRequest.Migrations
{
    [DbContext(typeof(SQLiteDBContext))]
    [Migration("20200413220849_summaryData Table add")]
    partial class summaryDataTableadd
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3");

            modelBuilder.Entity("WheaderRequest.AdressInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Class")
                        .HasColumnType("TEXT");

                    b.Property<string>("DisplayName")
                        .HasColumnType("TEXT");

                    b.Property<double>("Importance")
                        .HasColumnType("REAL");

                    b.Property<string>("Lat")
                        .HasColumnType("TEXT");

                    b.Property<string>("Licence")
                        .HasColumnType("TEXT");

                    b.Property<string>("Lon")
                        .HasColumnType("TEXT");

                    b.Property<string>("OsmId")
                        .HasColumnType("TEXT");

                    b.Property<string>("OsmType")
                        .HasColumnType("TEXT");

                    b.Property<string>("PlaceId")
                        .HasColumnType("TEXT");

                    b.Property<int>("RequestInfoId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Type")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("AdressInfos");
                });

            modelBuilder.Entity("WheaderRequest.SummaryData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("DailyMaxTemperature")
                        .HasColumnType("TEXT");

                    b.Property<string>("DailyMinTemperature")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DayDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Lat")
                        .HasColumnType("TEXT");

                    b.Property<string>("Lon")
                        .HasColumnType("TEXT");

                    b.Property<string>("PlaceId")
                        .HasColumnType("TEXT");

                    b.Property<string>("PlaceName")
                        .HasColumnType("TEXT");

                    b.Property<int>("ProcessId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("WeeklyMaxTemperature")
                        .HasColumnType("TEXT");

                    b.Property<string>("WeeklyMinTemperature")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("SummaryData");
                });

            modelBuilder.Entity("WheaderRequest.WsProcessInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("ProcessDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProcessName")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("WsProcessInfo");
                });
#pragma warning restore 612, 618
        }
    }
}
