using Microsoft.EntityFrameworkCore.Migrations;

namespace Employees.Migrations
{
    //* Part 50 - step1 - Install EntityFrameworkCore.Tools from the NuGet Package 
    //* Part 50 - step2 - Open Pakage manager Console from View-> OtherWindows
    //* Part 50 - step3 - get-help about_entityframeworkcore and get-help Add-Migration
    //* Part 50 - step4 - Add-Migration InitialMigration if nit work uninstall and install EntityFrameworkCore.SqlServer 3.1.5
    //* Part 50 - step5 - Update-Database (will create the EmployeeDB)
    //* Part 50 - step6 - View -> Sql Server Object Explorer -> EmployeeDB
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Department = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employees");
        }
    }
}
