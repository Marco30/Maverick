using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WarGamesAPI.Migrations
{
    /// <inheritdoc />
    public partial class ConnectLibraryConversationToConversation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChatHistoryConversationId",
                table: "LibraryConversation",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LibraryConversation_ChatHistoryConversationId",
                table: "LibraryConversation",
                column: "ChatHistoryConversationId");

            migrationBuilder.AddForeignKey(
                name: "FK_LibraryConversation_Conversation_ChatHistoryConversationId",
                table: "LibraryConversation",
                column: "ChatHistoryConversationId",
                principalTable: "Conversation",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LibraryConversation_Conversation_ChatHistoryConversationId",
                table: "LibraryConversation");

            migrationBuilder.DropIndex(
                name: "IX_LibraryConversation_ChatHistoryConversationId",
                table: "LibraryConversation");

            migrationBuilder.DropColumn(
                name: "ChatHistoryConversationId",
                table: "LibraryConversation");
        }
    }
}
