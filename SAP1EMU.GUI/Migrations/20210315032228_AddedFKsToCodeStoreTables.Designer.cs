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
    [Migration("20210315032228_AddedFKsToCodeStoreTables")]
    partial class AddedFKsToCodeStoreTables
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

                    b.Property<int>("EmulatorId")
                        .HasColumnType("int");

                    b.Property<int>("InstructionSetId")
                        .HasColumnType("int");

                    b.Property<DateTime>("SessionEnd")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("SessionStart")
                        .HasColumnType("datetime2");

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

            modelBuilder.Entity("SAP1EMU.Data.Lib.InstructionSet", b =>
                {
                    b.HasOne("SAP1EMU.Data.Lib.Emulator", "Emulator")
                        .WithMany("InstructionSets")
                        .HasForeignKey("EmulatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
