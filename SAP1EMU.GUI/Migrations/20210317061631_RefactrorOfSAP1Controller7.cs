using Microsoft.EntityFrameworkCore.Migrations;

namespace SAP1EMU.GUI.Migrations
{
    public partial class RefactrorOfSAP1Controller7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_EmulationSessionMaps_Status_StatusId1",
            //    table: "EmulationSessionMaps");

            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_EmulationSessionMaps",
            //    table: "EmulationSessionMaps");

            //migrationBuilder.DropIndex(
            //    name: "IX_EmulationSessionMaps_StatusId1",
            //    table: "EmulationSessionMaps");

            //migrationBuilder.DropColumn(
            //    name: "StatusId1",
            //    table: "EmulationSessionMaps");

            //migrationBuilder.AlterColumn<int>(
            //    name: "Id",
            //    table: "EmulationSessionMaps",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldType: "int")
            //    .Annotation("SqlServer:Identity", "1, 1");

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_EmulationSessionMaps",
            //    table: "EmulationSessionMaps",
            //    column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EmulationSessionMaps",
                table: "EmulationSessionMaps");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "EmulationSessionMaps",
                type: "int",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "StatusId1",
                table: "EmulationSessionMaps",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmulationSessionMaps",
                table: "EmulationSessionMaps",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_EmulationSessionMaps_StatusId1",
                table: "EmulationSessionMaps",
                column: "StatusId1");

            migrationBuilder.AddForeignKey(
                name: "FK_EmulationSessionMaps_Status_StatusId1",
                table: "EmulationSessionMaps",
                column: "StatusId1",
                principalTable: "Status",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
