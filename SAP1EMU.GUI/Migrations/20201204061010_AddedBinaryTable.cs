using Microsoft.EntityFrameworkCore.Migrations;

namespace SAP1EMU.GUI.Migrations
{
    public partial class AddedBinaryTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SAP2CodeStore_EmulationSessionMaps_EmulationSessionMapId",
                table: "SAP2CodeStore");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SAP2CodeStore",
                table: "SAP2CodeStore");

            migrationBuilder.RenameTable(
                name: "SAP2CodeStore",
                newName: "SAP2CodePacket");

            migrationBuilder.RenameIndex(
                name: "IX_SAP2CodeStore_EmulationSessionMapId",
                table: "SAP2CodePacket",
                newName: "IX_SAP2CodePacket_EmulationSessionMapId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SAP2CodePacket",
                table: "SAP2CodePacket",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SAP2CodePacket_EmulationSessionMaps_EmulationSessionMapId",
                table: "SAP2CodePacket",
                column: "EmulationSessionMapId",
                principalTable: "EmulationSessionMaps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SAP2CodePacket_EmulationSessionMaps_EmulationSessionMapId",
                table: "SAP2CodePacket");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SAP2CodePacket",
                table: "SAP2CodePacket");

            migrationBuilder.RenameTable(
                name: "SAP2CodePacket",
                newName: "SAP2CodeStore");

            migrationBuilder.RenameIndex(
                name: "IX_SAP2CodePacket_EmulationSessionMapId",
                table: "SAP2CodeStore",
                newName: "IX_SAP2CodeStore_EmulationSessionMapId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SAP2CodeStore",
                table: "SAP2CodeStore",
                column: "Id");

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
