using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SAP1EMU.GUI.Migrations
{
    public partial class TestFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CodeStore",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(nullable: true),
                    submitted_at = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodeStore", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "SAP2CodeStore",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmulationID = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    EmulationSessionMapId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SAP2CodeStore", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SAP2CodeStore_EmulationSessionMaps_EmulationSessionMapId",
                        column: x => x.EmulationSessionMapId,
                        principalTable: "EmulationSessionMaps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SAP2CodeStore_EmulationSessionMapId",
                table: "SAP2CodeStore",
                column: "EmulationSessionMapId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CodeStore");

            migrationBuilder.DropTable(
                name: "SAP2CodeStore");
        }
    }
}
