using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WarGamesAPI.Migrations
{
    /// <inheritdoc />
    public partial class RemoveAnswerFromLibraryConversation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LibraryAnswer_LibraryConversation_LibraryConversationId",
                table: "LibraryAnswer");

            migrationBuilder.DropIndex(
                name: "IX_LibraryAnswer_LibraryConversationId",
                table: "LibraryAnswer");

            migrationBuilder.DropColumn(
                name: "LibraryConversationId",
                table: "LibraryAnswer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LibraryConversationId",
                table: "LibraryAnswer",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LibraryAnswer_LibraryConversationId",
                table: "LibraryAnswer",
                column: "LibraryConversationId");

            migrationBuilder.AddForeignKey(
                name: "FK_LibraryAnswer_LibraryConversation_LibraryConversationId",
                table: "LibraryAnswer",
                column: "LibraryConversationId",
                principalTable: "LibraryConversation",
                principalColumn: "Id");
        }
    }
}
