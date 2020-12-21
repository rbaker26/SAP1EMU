using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SAP1EMU.GUI.Migrations
{
    public partial class AddedStatus4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "EmulationID",
                table: "SAP1ErrorLog",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "EmulationId",
                table: "CodeStore",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmulationID",
                table: "SAP1ErrorLog");

            migrationBuilder.DropColumn(
                name: "EmulationId",
                table: "CodeStore");
        }
    }
}
