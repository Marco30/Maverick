using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WarGamesAPI.Migrations
{
    /// <inheritdoc />
    public partial class RenameTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SavedAnswer");

            migrationBuilder.DropTable(
                name: "SavedQuestion");

            migrationBuilder.DropTable(
                name: "SavedConversation");

            migrationBuilder.CreateTable(
                name: "LibraryConversation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibraryConversation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LibraryConversation_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LibraryQuestion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LibraryConversationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibraryQuestion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LibraryQuestion_LibraryConversation_LibraryConversationId",
                        column: x => x.LibraryConversationId,
                        principalTable: "LibraryConversation",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LibraryQuestion_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LibraryAnswer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LibraryQuestionId = table.Column<int>(type: "int", nullable: true),
                    LibraryConversationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibraryAnswer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LibraryAnswer_LibraryConversation_LibraryConversationId",
                        column: x => x.LibraryConversationId,
                        principalTable: "LibraryConversation",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LibraryAnswer_LibraryQuestion_LibraryQuestionId",
                        column: x => x.LibraryQuestionId,
                        principalTable: "LibraryQuestion",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_LibraryAnswer_LibraryConversationId",
                table: "LibraryAnswer",
                column: "LibraryConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryAnswer_LibraryQuestionId",
                table: "LibraryAnswer",
                column: "LibraryQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryConversation_UserId",
                table: "LibraryConversation",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryQuestion_LibraryConversationId",
                table: "LibraryQuestion",
                column: "LibraryConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryQuestion_UserId",
                table: "LibraryQuestion",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LibraryAnswer");

            migrationBuilder.DropTable(
                name: "LibraryQuestion");

            migrationBuilder.DropTable(
                name: "LibraryConversation");

            migrationBuilder.CreateTable(
                name: "SavedConversation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SavedConversation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SavedConversation_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SavedQuestion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SavedConversationId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SavedQuestion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SavedQuestion_SavedConversation_SavedConversationId",
                        column: x => x.SavedConversationId,
                        principalTable: "SavedConversation",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SavedQuestion_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SavedAnswer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SavedConversationId = table.Column<int>(type: "int", nullable: true),
                    SavedQuestionId = table.Column<int>(type: "int", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SavedAnswer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SavedAnswer_SavedConversation_SavedConversationId",
                        column: x => x.SavedConversationId,
                        principalTable: "SavedConversation",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SavedAnswer_SavedQuestion_SavedQuestionId",
                        column: x => x.SavedQuestionId,
                        principalTable: "SavedQuestion",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SavedAnswer_SavedConversationId",
                table: "SavedAnswer",
                column: "SavedConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_SavedAnswer_SavedQuestionId",
                table: "SavedAnswer",
                column: "SavedQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_SavedConversation_UserId",
                table: "SavedConversation",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SavedQuestion_SavedConversationId",
                table: "SavedQuestion",
                column: "SavedConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_SavedQuestion_UserId",
                table: "SavedQuestion",
                column: "UserId");
        }
    }
}
