using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WarGamesAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddLibraryKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChatHistoryQuestionId",
                table: "LibraryQuestion",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ChatHistoryAnswerId",
                table: "LibraryAnswer",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LibraryQuestion_ChatHistoryQuestionId",
                table: "LibraryQuestion",
                column: "ChatHistoryQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryAnswer_ChatHistoryAnswerId",
                table: "LibraryAnswer",
                column: "ChatHistoryAnswerId");

            migrationBuilder.AddForeignKey(
                name: "FK_LibraryAnswer_Answer_ChatHistoryAnswerId",
                table: "LibraryAnswer",
                column: "ChatHistoryAnswerId",
                principalTable: "Answer",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_LibraryQuestion_Question_ChatHistoryQuestionId",
                table: "LibraryQuestion",
                column: "ChatHistoryQuestionId",
                principalTable: "Question",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LibraryAnswer_Answer_ChatHistoryAnswerId",
                table: "LibraryAnswer");

            migrationBuilder.DropForeignKey(
                name: "FK_LibraryQuestion_Question_ChatHistoryQuestionId",
                table: "LibraryQuestion");

            migrationBuilder.DropIndex(
                name: "IX_LibraryQuestion_ChatHistoryQuestionId",
                table: "LibraryQuestion");

            migrationBuilder.DropIndex(
                name: "IX_LibraryAnswer_ChatHistoryAnswerId",
                table: "LibraryAnswer");

            migrationBuilder.DropColumn(
                name: "ChatHistoryQuestionId",
                table: "LibraryQuestion");

            migrationBuilder.DropColumn(
                name: "ChatHistoryAnswerId",
                table: "LibraryAnswer");
        }
    }
}
