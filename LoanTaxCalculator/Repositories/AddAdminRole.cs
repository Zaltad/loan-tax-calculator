using Microsoft.EntityFrameworkCore.Migrations;

namespace LoanTaxCalculator.Repositories
{
    public partial class AddAdminRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"INSERT INTO [dbo].[AspNetRoles](Name, NormalizedName) 
                VALUES ('Admin', 'ADMIN')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DELETE FROM [dbo].[AspNetRoles] WHERE [NormalizedName] = 'ADMIN'");
        }
    }
}
