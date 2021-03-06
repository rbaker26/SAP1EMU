﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SAP1EMU.GUI.Contexts;

namespace SAP1EMU.GUI.Migrations
{
    [DbContext(typeof(Sap1EmuContext))]
    [Migration("20210315023608_UpdatedColNames")]
    partial class UpdatedColNames
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SAP1EMU.Data.Lib.CodeSubmit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("SubmittedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("CodeStore");
                });

            modelBuilder.Entity("SAP1EMU.Data.Lib.EmulationSessionMap", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ConnectionID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("EmulationID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("SessionEnd")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("SessionStart")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("EmulationSessionMaps");
                });

            modelBuilder.Entity("SAP1EMU.Data.Lib.SAP2BinaryPacket", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("EmulationID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("SetName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("SAP2BinaryStore");
                });

            modelBuilder.Entity("SAP1EMU.Data.Lib.SAP2CodePacket", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("EmulationID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("SetName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("SAP2CodeStore");
                });
#pragma warning restore 612, 618
        }
    }
}
