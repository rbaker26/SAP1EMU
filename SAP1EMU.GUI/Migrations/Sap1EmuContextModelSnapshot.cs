﻿// <auto-generated />
using System;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

using SAP1EMU.GUI.Contexts;

namespace SAP1EMU.GUI.Migrations
{
    [DbContext(typeof(Sap1EmuContext))]
    partial class Sap1EmuContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.5")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SAP1EMU.Data.Lib.CodeSubmission", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                b.Property<string>("Code")
                    .HasColumnType("nvarchar(max)");

                b.Property<Guid>("EmulationID")
                    .HasColumnType("uniqueidentifier");

                b.HasKey("Id");

                b.ToTable("CodeSubmission");
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

                b.Property<int>("EmulatorId")
                    .HasColumnType("int");

                b.Property<int>("InstructionSetId")
                    .HasColumnType("int");

                b.Property<DateTime>("SessionEnd")
                    .HasColumnType("datetime2");

                b.Property<DateTime>("SessionStart")
                    .HasColumnType("datetime2");

                b.Property<int>("StatusId")
                    .HasColumnType("int");

                b.HasKey("Id");

                b.ToTable("EmulationSessionMaps");
            });

            modelBuilder.Entity("SAP1EMU.Data.Lib.Emulator", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                b.Property<string>("Name")
                    .HasColumnType("nvarchar(max)");

                b.HasKey("Id");

                b.ToTable("Emulators");
            });

            modelBuilder.Entity("SAP1EMU.Data.Lib.ErrorLog", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                b.Property<Guid>("EmulationID")
                    .HasColumnType("uniqueidentifier");

                b.Property<string>("ErrorMsg")
                    .HasColumnType("nvarchar(max)");

                b.HasKey("Id");

                b.ToTable("ErrorLog");
            });

            modelBuilder.Entity("SAP1EMU.Data.Lib.InstructionSet", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                b.Property<int>("EmulatorId")
                    .HasColumnType("int");

                b.Property<string>("Name")
                    .HasColumnType("nvarchar(max)");

                b.HasKey("Id");

                b.HasIndex("EmulatorId");

                b.ToTable("InstructionSets");
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

                b.ToTable("SAP2BinaryPacket");
            });

            modelBuilder.Entity("SAP1EMU.Data.Lib.SAP2CodePacket", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                b.Property<string>("Code")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.Property<Guid>("EmulationID")
                    .HasColumnType("uniqueidentifier");

                b.Property<string>("SetName")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.HasKey("Id");

                b.ToTable("SAP2CodePacket");
            });

            modelBuilder.Entity("SAP1EMU.Data.Lib.Security.Threat", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                b.Property<string>("Description")
                    .HasColumnType("nvarchar(max)");

                b.HasKey("Id");

                b.ToTable("Threats");
            });

            modelBuilder.Entity("SAP1EMU.Data.Lib.Security.ThreatLog", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                b.Property<string>("ClientIpAddress")
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("QueryString")
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("ThreatContent")
                    .HasColumnType("nvarchar(max)");

                b.Property<Guid>("ThreatIdentifier")
                    .HasColumnType("uniqueidentifier");

                b.Property<int>("ThreatTypeId")
                    .HasColumnType("int");

                b.Property<string>("Url")
                    .HasColumnType("nvarchar(max)");

                b.HasKey("Id");

                b.ToTable("ThreatLogs");
            });

            modelBuilder.Entity("SAP1EMU.Data.Lib.Status", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                b.Property<string>("Description")
                    .HasColumnType("nvarchar(max)");

                b.HasKey("Id");

                b.ToTable("Status");
            });

            modelBuilder.Entity("SAP1EMU.Data.Lib.InstructionSet", b =>
            {
                b.HasOne("SAP1EMU.Data.Lib.Emulator", "Emulator")
                    .WithMany("InstructionSets")
                    .HasForeignKey("EmulatorId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("Emulator");
            });

            modelBuilder.Entity("SAP1EMU.Data.Lib.Emulator", b =>
            {
                b.Navigation("InstructionSets");
            });
#pragma warning restore 612, 618
        }
    }
}