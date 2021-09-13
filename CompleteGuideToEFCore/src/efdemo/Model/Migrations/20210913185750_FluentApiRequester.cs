using Microsoft.EntityFrameworkCore.Migrations;

namespace Model.Migrations
{
    public partial class FluentApiRequester : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExpenseHeaders_Users_RequesterId",
                table: "ExpenseHeaders");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseHeaders_Users_RequesterId",
                table: "ExpenseHeaders",
                column: "RequesterId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExpenseHeaders_Users_RequesterId",
                table: "ExpenseHeaders");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseHeaders_Users_RequesterId",
                table: "ExpenseHeaders",
                column: "RequesterId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
