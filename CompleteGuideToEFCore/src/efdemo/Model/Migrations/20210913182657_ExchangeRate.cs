using Microsoft.EntityFrameworkCore.Migrations;

namespace Model.Migrations
{
    public partial class ExchangeRate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "UsdExchangeRate",
                table: "ExpenseHeaders",
                type: "decimal(13,4)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UsdExchangeRate",
                table: "ExpenseHeaders");
        }
    }
}
