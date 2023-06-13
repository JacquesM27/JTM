using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JTM.Migrations
{
    public partial class typosFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartOdWorkingDate",
                table: "WorkingTimes",
                newName: "StartOfWorkingDate");

            migrationBuilder.RenameColumn(
                name: "EndOdWorkingDate",
                table: "WorkingTimes",
                newName: "EndOfWorkingDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartOfWorkingDate",
                table: "WorkingTimes",
                newName: "StartOdWorkingDate");

            migrationBuilder.RenameColumn(
                name: "EndOfWorkingDate",
                table: "WorkingTimes",
                newName: "EndOdWorkingDate");
        }
    }
}
