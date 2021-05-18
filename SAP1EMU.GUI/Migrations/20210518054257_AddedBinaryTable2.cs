using Microsoft.EntityFrameworkCore.Migrations;

namespace SAP1EMU.GUI.Migrations
{
    public partial class AddedBinaryTable2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CodeSubmissions",
                table: "CodeSubmissions");

            migrationBuilder.RenameTable(
                name: "CodeSubmissions",
                newName: "CodeSubmission");

            migrationBuilder.AlterColumn<string>(
                name: "SetName",
                table: "SAP2CodeStore",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "SAP2CodeStore",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CodeSubmission",
                table: "CodeSubmission",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CodeSubmission",
                table: "CodeSubmission");

            migrationBuilder.RenameTable(
                name: "CodeSubmission",
                newName: "CodeSubmissions");

            migrationBuilder.AlterColumn<string>(
                name: "SetName",
                table: "SAP2CodeStore",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "SAP2CodeStore",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CodeSubmissions",
                table: "CodeSubmissions",
                column: "Id");
        }
    }
}
