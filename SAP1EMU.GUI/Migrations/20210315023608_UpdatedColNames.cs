using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SAP1EMU.GUI.Migrations
{
    public partial class UpdatedColNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "submitted_at",
                table: "CodeStore");

            migrationBuilder.RenameColumn(
                name: "code",
                table: "CodeStore",
                newName: "Code");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "CodeStore",
                newName: "Id");

            migrationBuilder.AddColumn<string>(
                name: "SetName",
                table: "SAP2CodeStore",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SetName",
                table: "SAP2BinaryStore",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SubmittedAt",
                table: "CodeStore",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SetName",
                table: "SAP2CodeStore");

            migrationBuilder.DropColumn(
                name: "SetName",
                table: "SAP2BinaryStore");

            migrationBuilder.DropColumn(
                name: "SubmittedAt",
                table: "CodeStore");

            migrationBuilder.RenameColumn(
                name: "Code",
                table: "CodeStore",
                newName: "code");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "CodeStore",
                newName: "id");

            migrationBuilder.AddColumn<DateTime>(
                name: "submitted_at",
                table: "CodeStore",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
