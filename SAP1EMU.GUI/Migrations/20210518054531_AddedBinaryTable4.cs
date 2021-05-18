using Microsoft.EntityFrameworkCore.Migrations;

namespace SAP1EMU.GUI.Migrations
{
    public partial class AddedBinaryTable4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SAP2CodeStore",
                table: "SAP2CodeStore");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SAP2BinaryStore",
                table: "SAP2BinaryStore");

            migrationBuilder.RenameTable(
                name: "SAP2CodeStore",
                newName: "SAP2CodePacket");

            migrationBuilder.RenameTable(
                name: "SAP2BinaryStore",
                newName: "SAP2BinaryPacket");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SAP2CodePacket",
                table: "SAP2CodePacket",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SAP2BinaryPacket",
                table: "SAP2BinaryPacket",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SAP2CodePacket",
                table: "SAP2CodePacket");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SAP2BinaryPacket",
                table: "SAP2BinaryPacket");

            migrationBuilder.RenameTable(
                name: "SAP2CodePacket",
                newName: "SAP2CodeStore");

            migrationBuilder.RenameTable(
                name: "SAP2BinaryPacket",
                newName: "SAP2BinaryStore");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SAP2CodeStore",
                table: "SAP2CodeStore",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SAP2BinaryStore",
                table: "SAP2BinaryStore",
                column: "Id");
        }
    }
}
