using Microsoft.EntityFrameworkCore.Migrations;

namespace SAP1EMU.GUI.Migrations
{
    public partial class RefactrorOfSAP1Controller9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "EmulationSessionMaps",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "EmulationSessionMaps");
        }
    }
}
