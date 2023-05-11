using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WarGamesAPI.Migrations
{
    /// <inheritdoc />
    public partial class RemoveRelationshipAnswerToConversation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answer_Conversation_ConversationId",
                table: "Answer");

            migrationBuilder.DropIndex(
                name: "IX_Answer_ConversationId",
                table: "Answer");

            migrationBuilder.DropColumn(
                name: "ConversationId",
                table: "Answer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ConversationId",
                table: "Answer",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Answer_ConversationId",
                table: "Answer",
                column: "ConversationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Answer_Conversation_ConversationId",
                table: "Answer",
                column: "ConversationId",
                principalTable: "Conversation",
                principalColumn: "Id");
        }
    }
}
