using Microsoft.EntityFrameworkCore.Migrations;

namespace Model.Migrations
{
    public partial class UserToExpenseHeader : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ApproverId",
                table: "ExpenseHeaders",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RequesterId",
                table: "ExpenseHeaders",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseHeaders_ApproverId",
                table: "ExpenseHeaders",
                column: "ApproverId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseHeaders_RequesterId",
                table: "ExpenseHeaders",
                column: "RequesterId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseHeaders_Users_ApproverId",
                table: "ExpenseHeaders",
                column: "ApproverId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseHeaders_Users_RequesterId",
                table: "ExpenseHeaders",
                column: "RequesterId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExpenseHeaders_Users_ApproverId",
                table: "ExpenseHeaders");

            migrationBuilder.DropForeignKey(
                name: "FK_ExpenseHeaders_Users_RequesterId",
                table: "ExpenseHeaders");

            migrationBuilder.DropIndex(
                name: "IX_ExpenseHeaders_ApproverId",
                table: "ExpenseHeaders");

            migrationBuilder.DropIndex(
                name: "IX_ExpenseHeaders_RequesterId",
                table: "ExpenseHeaders");

            migrationBuilder.DropColumn(
                name: "ApproverId",
                table: "ExpenseHeaders");

            migrationBuilder.DropColumn(
                name: "RequesterId",
                table: "ExpenseHeaders");
        }
    }
}
