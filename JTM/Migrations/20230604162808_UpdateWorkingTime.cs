using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JTM.Migrations
{
    public partial class UpdateWorkingTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkingTimes_Companies_CompanyId",
                table: "WorkingTimes");

            migrationBuilder.RenameColumn(
                name: "WorkingDate",
                table: "WorkingTimes",
                newName: "StartOdWorkingDate");

            migrationBuilder.RenameColumn(
                name: "Minutes",
                table: "WorkingTimes",
                newName: "LastEditorId");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "WorkingTimes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "AuthorId",
                table: "WorkingTimes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "WorkingTimes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndOdWorkingDate",
                table: "WorkingTimes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_WorkingTimes_AuthorId",
                table: "WorkingTimes",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkingTimes_LastEditorId",
                table: "WorkingTimes",
                column: "LastEditorId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkingTimes_Companies_CompanyId",
                table: "WorkingTimes",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkingTimes_Users_AuthorId",
                table: "WorkingTimes",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkingTimes_Users_LastEditorId",
                table: "WorkingTimes",
                column: "LastEditorId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkingTimes_Companies_CompanyId",
                table: "WorkingTimes");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkingTimes_Users_AuthorId",
                table: "WorkingTimes");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkingTimes_Users_LastEditorId",
                table: "WorkingTimes");

            migrationBuilder.DropIndex(
                name: "IX_WorkingTimes_AuthorId",
                table: "WorkingTimes");

            migrationBuilder.DropIndex(
                name: "IX_WorkingTimes_LastEditorId",
                table: "WorkingTimes");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "WorkingTimes");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "WorkingTimes");

            migrationBuilder.DropColumn(
                name: "EndOdWorkingDate",
                table: "WorkingTimes");

            migrationBuilder.RenameColumn(
                name: "StartOdWorkingDate",
                table: "WorkingTimes",
                newName: "WorkingDate");

            migrationBuilder.RenameColumn(
                name: "LastEditorId",
                table: "WorkingTimes",
                newName: "Minutes");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "WorkingTimes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkingTimes_Companies_CompanyId",
                table: "WorkingTimes",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
