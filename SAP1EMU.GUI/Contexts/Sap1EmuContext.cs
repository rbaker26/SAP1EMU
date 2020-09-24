using Microsoft.EntityFrameworkCore;
using SAP1EMU.GUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAP1EMU.GUI.Contexts
{
    public class Sap1EmuContext : DbContext
    {
        public Sap1EmuContext(DbContextOptions<Sap1EmuContext> options)
            : base(options)
        {
            
        }

        public DbSet<CodeSubmit> Code { get; set; }


    }
}
