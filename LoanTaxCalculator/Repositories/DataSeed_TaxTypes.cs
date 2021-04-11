using Microsoft.EntityFrameworkCore.Migrations;

namespace LoanTaxCalculator.Repositories
{
    public partial class DataSeed_TaxTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO [dbo].[TaxTypes]([Type], [UnitOfMeasurement]) VALUES ('Elektra (naktinis)', 'kWh')");
            migrationBuilder.Sql("INSERT INTO [dbo].[TaxTypes]([Type], [UnitOfMeasurement]) VALUES ('Elektra (dieninis)', 'kWh')");
            migrationBuilder.Sql("INSERT INTO [dbo].[TaxTypes]([Type], [UnitOfMeasurement]) VALUES ('Karštas vanduo', 'm3')");
            migrationBuilder.Sql("INSERT INTO [dbo].[TaxTypes]([Type], [UnitOfMeasurement]) VALUES ('Šaltas vanduo', 'm3')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [dbo].[TaxTypes] WHERE Type = 'Elektra (naktinis)'");
            migrationBuilder.Sql("DELETE FROM [dbo].[TaxTypes] WHERE Type = 'Elektra (dieninis)'");
            migrationBuilder.Sql("DELETE FROM [dbo].[TaxTypes] WHERE Type = 'Karštas vanduo'");
            migrationBuilder.Sql("DELETE FROM [dbo].[TaxTypes] WHERE Type = 'Šaltas vanduo'");
        }
    }
}
