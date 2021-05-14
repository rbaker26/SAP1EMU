using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

using SAP1EMU.Data.Lib;
using SAP1EMU.Data.Lib.Security;

using System.Collections.Generic;
//using System.Linq; // Needed for EF Migration ?

namespace SAP1EMU.GUI.Contexts
{
    public class Sap1EmuContext : DbContext
    {
        public Sap1EmuContext(DbContextOptions<Sap1EmuContext> options)
            : base(options)
        {
        }
        public DbSet<Emulator> Emulators { get; set; }
        public DbSet<InstructionSet> InstructionSets { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<EmulationSessionMap> EmulationSessionMaps { get; set; }
        public DbSet<CodeSubmission> CodeSubmissions { get; set; }
        public DbSet<ErrorLog> ErrorLog { get; set; }


        // SAP1
        //public DbSet<CodeSubmit> CodeStore { get; set; }


        // SAP2 
        public DbSet<SAP2CodePacket> SAP2CodeStore { get; set; }
        public DbSet<SAP2BinaryPacket> SAP2BinaryStore { get; set; }


        // Threats & Security
        public DbSet<ThreatLog> ThreatLogs { get; set; }
        public DbSet<Threat> Threats { get; set; }



        // Coverts Lists of Strings into a string for EF Core
        protected override void OnModelCreating(ModelBuilder builder)
        {
            var splitStringConverter = new ValueConverter<IEnumerable<string>, string>(v => string.Join(";", v), v => v.Split(new[] { ';' }));
            builder.Entity<SAP2CodePacket>().Property(nameof(SAP2CodePacket.Code)).HasConversion(splitStringConverter);
            builder.Entity<SAP2BinaryPacket>().Property(nameof(SAP2BinaryPacket.Code)).HasConversion(splitStringConverter);
            builder.Entity<CodeSubmission>().Property(nameof(CodeSubmission.Code)).HasConversion(splitStringConverter);
        }

    }
}