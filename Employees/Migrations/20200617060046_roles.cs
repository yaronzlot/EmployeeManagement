using Microsoft.EntityFrameworkCore.Migrations;

namespace Employees.Migrations
{
    public partial class roles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Department",
                table: "Employees");

            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: "Employees",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Name", "Role" },
                values: new object[] { "Michael", 0 });

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
                columns: new[] { "Email", "Name", "Role" },
                values: new object[] { "scottie@astea.com", "Scottie", 2 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "Employees");

            migrationBuilder.AddColumn<int>(
                name: "Department",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Department", "Name" },
                values: new object[] { 2, "Bob" });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Department", "Email", "Name" },
                values: new object[] { 1, "john@astea.com", "John" });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Department", "Email", "Name" },
                values: new object[] { 3, "david@astea.com", "David" });
        }
    }
}
