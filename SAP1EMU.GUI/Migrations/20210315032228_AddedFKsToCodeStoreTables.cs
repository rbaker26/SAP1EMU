using Microsoft.EntityFrameworkCore.Migrations;

namespace SAP1EMU.GUI.Migrations
{
    public partial class AddedFKsToCodeStoreTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmulatorId",
                table: "EmulationSessionMaps",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "InstructionSetId",
                table: "EmulationSessionMaps",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmulatorId",
                table: "EmulationSessionMaps");

            migrationBuilder.DropColumn(
                name: "InstructionSetId",
                table: "EmulationSessionMaps");
        }
    }
}
