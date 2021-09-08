using Microsoft.EntityFrameworkCore.Migrations;

namespace Model.Migrations
{
    public partial class RenameExpenseLine : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ExpenseDetails",
                table: "ExpenseDetails");

            migrationBuilder.RenameTable(
                name: "ExpenseDetails",
                newName: "ExpenseLines");

            migrationBuilder.RenameColumn(
                name: "UnitPrice",
                table: "ExpenseLines",
                newName: "UnitCost");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExpenseLines",
                table: "ExpenseLines",
                column: "ExpenseLineId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ExpenseLines",
                table: "ExpenseLines");

            migrationBuilder.RenameTable(
                name: "ExpenseLines",
                newName: "ExpenseDetails");

            migrationBuilder.RenameColumn(
                name: "UnitCost",
                table: "ExpenseDetails",
                newName: "UnitPrice");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExpenseDetails",
                table: "ExpenseDetails",
                column: "ExpenseLineId");
        }
    }
}
