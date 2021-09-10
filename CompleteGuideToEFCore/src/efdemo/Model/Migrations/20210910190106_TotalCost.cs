using Microsoft.EntityFrameworkCore.Migrations;

namespace Model.Migrations
{
    public partial class TotalCost : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TotalCost",
                table: "ExpenseLines",
                type: "decimal(16, 2)",
                nullable: false,
                computedColumnSql: "[Quantity] * [UnitCost]");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalCost",
                table: "ExpenseLines");
        }
    }
}
