using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WarGamesAPI.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence<int>(
                name: "SharedIdSequence",
                startValue: 101L);

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SocialSecurityNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MobilePhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AgreeMarketing = table.Column<bool>(type: "bit", nullable: false),
                    SubscribeToEmailNotification = table.Column<bool>(type: "bit", nullable: false),
                    ProfileImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ZipCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Attention = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Municipality = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CareOf = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Address_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Conversation",
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
                    table.PrimaryKey("PK_Conversation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Conversation_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LibraryConversation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ChatHistoryConversationId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibraryConversation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LibraryConversation_Conversation_ChatHistoryConversationId",
                        column: x => x.ChatHistoryConversationId,
                        principalTable: "Conversation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_LibraryConversation_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Question",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, defaultValueSql: "NEXT VALUE FOR SharedIdSequence"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ConversationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Question", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Question_Conversation_ConversationId",
                        column: x => x.ConversationId,
                        principalTable: "Conversation",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Question_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Answer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, defaultValueSql: "NEXT VALUE FOR SharedIdSequence"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Answer_Question_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LibraryQuestion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ChatHistoryQuestionId = table.Column<int>(type: "int", nullable: true),
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
                        name: "FK_LibraryQuestion_Question_ChatHistoryQuestionId",
                        column: x => x.ChatHistoryQuestionId,
                        principalTable: "Question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
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
                    ChatHistoryAnswerId = table.Column<int>(type: "int", nullable: true),
                    LibraryQuestionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibraryAnswer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LibraryAnswer_Answer_ChatHistoryAnswerId",
                        column: x => x.ChatHistoryAnswerId,
                        principalTable: "Answer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_LibraryAnswer_LibraryQuestion_LibraryQuestionId",
                        column: x => x.LibraryQuestionId,
                        principalTable: "LibraryQuestion",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "AgreeMarketing", "Email", "FirstName", "FullName", "Gender", "LastName", "MobilePhoneNumber", "Password", "PhoneNumber", "ProfileImage", "SocialSecurityNumber", "SubscribeToEmailNotification" },
                values: new object[] { 5, true, "khaled@khaled.se", "Khaled", "Khaled Abo", "Man", "Abo", null, "123456", null, null, "198507119595", true });

            migrationBuilder.InsertData(
                table: "Address",
                columns: new[] { "UserId", "Attention", "CareOf", "City", "Country", "Municipality", "Street", "ZipCode" },
                values: new object[] { 5, null, null, "Stockholm", "Sweden", null, "R�ntgenv�gen 5 lgh 1410", "14152" });

            migrationBuilder.InsertData(
                table: "Conversation",
                columns: new[] { "Id", "Date", "Name", "Updated", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Fun chat", new DateTime(2023, 5, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 5 },
                    { 2, new DateTime(2023, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Software Questions", new DateTime(2023, 5, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 5 },
                    { 3, new DateTime(2023, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "My first Conversation", new DateTime(2023, 3, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 5 },
                    { 4, new DateTime(2023, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "My second Conversation", new DateTime(2023, 3, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 5 },
                    { 5, new DateTime(2023, 2, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "My third Conversation", new DateTime(2023, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 5 },
                    { 6, new DateTime(2023, 2, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Fourth one here", new DateTime(2023, 3, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 5 },
                    { 7, new DateTime(2023, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "A bit of business", new DateTime(2023, 3, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 5 },
                    { 8, new DateTime(2023, 2, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Some other topic", new DateTime(2023, 3, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 5 },
                    { 9, new DateTime(2023, 2, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Yet another chat", new DateTime(2023, 3, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 5 },
                    { 10, new DateTime(2023, 2, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Last but not least", new DateTime(2023, 3, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), 5 }
                });

            migrationBuilder.InsertData(
                table: "Question",
                columns: new[] { "Id", "ConversationId", "Date", "Text", "UserId" },
                values: new object[,]
                {
                    { 1, 3, new DateTime(2023, 3, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "How many babies can a rabbit have in its lifetime??", 5 },
                    { 2, 2, new DateTime(2023, 5, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "What is the difference between a struct and a class in C#?", 5 },
                    { 3, 1, new DateTime(2023, 5, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "What is AI?", 5 },
                    { 4, 4, new DateTime(2023, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "What is the capital of France?", 5 },
                    { 5, 5, new DateTime(2023, 2, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "How does a quantum computer work?", 5 },
                    { 6, 6, new DateTime(2023, 2, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "What is the meaning of life?", 5 },
                    { 7, 7, new DateTime(2023, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "What is the highest mountain in the world?", 5 },
                    { 8, 8, new DateTime(2023, 2, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "What's the square root of 144?", 5 },
                    { 9, 9, new DateTime(2023, 2, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Who wrote the novel '1984'?", 5 },
                    { 10, 10, new DateTime(2023, 2, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "What is the boiling point of water at sea level?", 5 },
                    { 11, 1, new DateTime(2023, 2, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "What is the fastest land animal?", 5 },
                    { 12, 2, new DateTime(2023, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "What is the distance between Earth and the Moon?", 5 },
                    { 13, 3, new DateTime(2023, 2, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "What's the chemical formula for water?", 5 }
                });

            migrationBuilder.InsertData(
                table: "Answer",
                columns: new[] { "Id", "Date", "QuestionId", "Text" },
                values: new object[,]
                {
                    { 14, new DateTime(2023, 2, 2, 0, 0, 1, 0, DateTimeKind.Unspecified), 4, "The capital of France is Paris." },
                    { 15, new DateTime(2023, 2, 3, 0, 0, 1, 0, DateTimeKind.Unspecified), 5, "A quantum computer works by leveraging phenomena in quantum mechanics like superposition and entanglement." },
                    { 16, new DateTime(2023, 2, 4, 0, 0, 1, 0, DateTimeKind.Unspecified), 6, "The meaning of life is subjective and can differ from person to person. It can be happiness, love, contribution, or something else entirely." },
                    { 17, new DateTime(2023, 2, 5, 0, 0, 1, 0, DateTimeKind.Unspecified), 7, "The highest mountain in the world is Mount Everest." },
                    { 18, new DateTime(2023, 2, 6, 0, 0, 1, 0, DateTimeKind.Unspecified), 8, "The square root of 144 is 12." },
                    { 19, new DateTime(2023, 2, 7, 0, 0, 1, 0, DateTimeKind.Unspecified), 9, "The novel '1984' was written by George Orwell." },
                    { 20, new DateTime(2023, 2, 8, 0, 0, 1, 0, DateTimeKind.Unspecified), 10, "The boiling point of water at sea level is approximately 100 degrees Celsius or 212 degrees Fahrenheit." },
                    { 21, new DateTime(2023, 2, 9, 0, 0, 1, 0, DateTimeKind.Unspecified), 11, "The fastest land animal is the cheetah." },
                    { 22, new DateTime(2023, 2, 10, 0, 0, 1, 0, DateTimeKind.Unspecified), 12, "The average distance between Earth and the Moon is about 238,855 miles." },
                    { 23, new DateTime(2023, 2, 11, 0, 0, 1, 0, DateTimeKind.Unspecified), 13, "The chemical formula for water is H2O." }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Answer_QuestionId",
                table: "Answer",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Conversation_UserId",
                table: "Conversation",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryAnswer_ChatHistoryAnswerId",
                table: "LibraryAnswer",
                column: "ChatHistoryAnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryAnswer_LibraryQuestionId",
                table: "LibraryAnswer",
                column: "LibraryQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryConversation_ChatHistoryConversationId",
                table: "LibraryConversation",
                column: "ChatHistoryConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryConversation_UserId",
                table: "LibraryConversation",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryQuestion_ChatHistoryQuestionId",
                table: "LibraryQuestion",
                column: "ChatHistoryQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryQuestion_LibraryConversationId",
                table: "LibraryQuestion",
                column: "LibraryConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryQuestion_UserId",
                table: "LibraryQuestion",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Question_ConversationId",
                table: "Question",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_Question_UserId",
                table: "Question",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "LibraryAnswer");

            migrationBuilder.DropTable(
                name: "Answer");

            migrationBuilder.DropTable(
                name: "LibraryQuestion");

            migrationBuilder.DropTable(
                name: "LibraryConversation");

            migrationBuilder.DropTable(
                name: "Question");

            migrationBuilder.DropTable(
                name: "Conversation");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropSequence(
                name: "SharedIdSequence");
        }
    }
}
