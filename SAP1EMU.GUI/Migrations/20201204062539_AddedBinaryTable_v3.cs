using Microsoft.EntityFrameworkCore.Migrations;

namespace SAP1EMU.GUI.Migrations
{
    public partial class AddedBinaryTable_v3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SAP2BinaryStore_EmulationSessionMaps_EmulationSessionMapId",
                table: "SAP2BinaryStore");

            migrationBuilder.DropForeignKey(
                name: "FK_SAP2CodeStore_EmulationSessionMaps_EmulationSessionMapId",
                table: "SAP2CodeStore");

            migrationBuilder.DropIndex(
                name: "IX_SAP2CodeStore_EmulationSessionMapId",
                table: "SAP2CodeStore");

            migrationBuilder.DropIndex(
                name: "IX_SAP2BinaryStore_EmulationSessionMapId",
                table: "SAP2BinaryStore");

            migrationBuilder.DropColumn(
                name: "EmulationSessionMapId",
                table: "SAP2CodeStore");

            migrationBuilder.DropColumn(
                name: "EmulationSessionMapId",
                table: "SAP2BinaryStore");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmulationSessionMapId",
                table: "SAP2CodeStore",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EmulationSessionMapId",
                table: "SAP2BinaryStore",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SAP2CodeStore_EmulationSessionMapId",
                table: "SAP2CodeStore",
                column: "EmulationSessionMapId");

            migrationBuilder.CreateIndex(
                name: "IX_SAP2BinaryStore_EmulationSessionMapId",
                table: "SAP2BinaryStore",
                column: "EmulationSessionMapId");

            migrationBuilder.AddForeignKey(
                name: "FK_SAP2BinaryStore_EmulationSessionMaps_EmulationSessionMapId",
                table: "SAP2BinaryStore",
                column: "EmulationSessionMapId",
                principalTable: "EmulationSessionMaps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SAP2CodeStore_EmulationSessionMaps_EmulationSessionMapId",
                table: "SAP2CodeStore",
                column: "EmulationSessionMapId",
                principalTable: "EmulationSessionMaps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
