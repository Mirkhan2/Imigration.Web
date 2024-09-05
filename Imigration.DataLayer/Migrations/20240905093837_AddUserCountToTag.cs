using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Imigration.DataLayer.Migrations
{
    public partial class AddUserCountToTag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SelectQuestionTags_Users_UserId",
                table: "SelectQuestionTags");

            migrationBuilder.DropIndex(
                name: "IX_SelectQuestionTags_UserId",
                table: "SelectQuestionTags");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "SelectQuestionTags");

            migrationBuilder.AddColumn<int>(
                name: "UseCount",
                table: "Tags",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UseCount",
                table: "Tags");

            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "SelectQuestionTags",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_SelectQuestionTags_UserId",
                table: "SelectQuestionTags",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SelectQuestionTags_Users_UserId",
                table: "SelectQuestionTags",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
