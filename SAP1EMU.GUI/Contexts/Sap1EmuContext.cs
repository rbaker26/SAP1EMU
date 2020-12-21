using Microsoft.EntityFrameworkCore;

using SAP1EMU.GUI.Models;

namespace SAP1EMU.GUI.Contexts
{
    public class Sap1EmuContext : DbContext
    {
        public Sap1EmuContext(DbContextOptions<Sap1EmuContext> options)
            : base(options)
        {
        }

        public DbSet<CodeSubmit> CodeStore { get; set; }
        public DbSet<SAP1ErrorLog> SAP1ErrorLog { get; set; }
    }
}