using Microsoft.EntityFrameworkCore.Migrations;

namespace Employees.Migrations
{
    public partial class fixseeddata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Email", "Name" },
                values: new object[] { "michael@nba.com", "Michael Jordan" });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Email", "Name", "Role" },
                values: new object[] { "dennis@nba.com", "Dennis Rodman", 1 });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Email", "Name" },
                values: new object[] { "scottie@nba.com", "Scottie Pippen" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Email", "Name" },
                values: new object[] { "bob@astea.com", "Michael" });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Email", "Name", "Role" },
                values: new object[] { "Dennis@astea.com", "Dennis", 2 });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Email", "Name" },
                values: new object[] { "scottie@astea.com", "Scottie" });
        }
    }
}
