using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JTM.Migrations
{
    public partial class newWtColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndOfWorkingDate",
                table: "WorkingTimes");

            migrationBuilder.RenameColumn(
                name: "StartOfWorkingDate",
                table: "WorkingTimes",
                newName: "WorkingDate");

            migrationBuilder.AddColumn<int>(
                name: "SecondsOfWork",
                table: "WorkingTimes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SecondsOfWork",
                table: "WorkingTimes");

            migrationBuilder.RenameColumn(
                name: "WorkingDate",
                table: "WorkingTimes",
                newName: "StartOfWorkingDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndOfWorkingDate",
                table: "WorkingTimes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
