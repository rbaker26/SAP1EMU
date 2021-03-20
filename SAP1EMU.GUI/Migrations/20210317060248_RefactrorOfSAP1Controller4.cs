using Microsoft.EntityFrameworkCore.Migrations;

namespace SAP1EMU.GUI.Migrations
{
    public partial class RefactrorOfSAP1Controller4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_EmulationSessionMaps_Status_StatusId",
            //    table: "EmulationSessionMaps");

            //migrationBuilder.DropTable(
            //    name: "Status");

            //migrationBuilder.DropIndex(
            //    name: "IX_EmulationSessionMaps_StatusId",
            //    table: "EmulationSessionMaps");

            //migrationBuilder.DropColumn(
            //    name: "StatusId",
            //    table: "EmulationSessionMaps");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<int>(
            //    name: "StatusId",
            //    table: "EmulationSessionMaps",
            //    type: "int",
            //    nullable: true);

            //migrationBuilder.CreateTable(
            //    name: "Status",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Status", x => x.Id);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_EmulationSessionMaps_StatusId",
            //    table: "EmulationSessionMaps",
            //    column: "StatusId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_EmulationSessionMaps_Status_StatusId",
            //    table: "EmulationSessionMaps",
            //    column: "StatusId",
            //    principalTable: "Status",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Restrict);
        }
    }
}
