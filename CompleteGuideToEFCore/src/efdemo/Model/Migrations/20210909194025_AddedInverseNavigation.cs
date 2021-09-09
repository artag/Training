using Microsoft.EntityFrameworkCore.Migrations;

namespace Model.Migrations
{
    public partial class AddedInverseNavigation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExpenseLines_ExpenseHeaders_ExpenseHeaderId",
                table: "ExpenseLines");

            migrationBuilder.AlterColumn<int>(
                name: "ExpenseHeaderId",
                table: "ExpenseLines",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseLines_ExpenseHeaders_ExpenseHeaderId",
                table: "ExpenseLines",
                column: "ExpenseHeaderId",
                principalTable: "ExpenseHeaders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExpenseLines_ExpenseHeaders_ExpenseHeaderId",
                table: "ExpenseLines");

            migrationBuilder.AlterColumn<int>(
                name: "ExpenseHeaderId",
                table: "ExpenseLines",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseLines_ExpenseHeaders_ExpenseHeaderId",
                table: "ExpenseLines",
                column: "ExpenseHeaderId",
                principalTable: "ExpenseHeaders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
