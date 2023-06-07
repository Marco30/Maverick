using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WarGamesAPI.Migrations
{
    /// <inheritdoc />
    public partial class QuestionAnswerView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE VIEW QuestionAnswer AS
SELECT 
    q.Id AS Id,
    q.UserId AS UserId,
    q.Text AS Text,
    'Question' AS Source,
    q.ConversationId AS ConversationId,
    DATEDIFF(SECOND, '19700101', q.Date) AS Timestamp
FROM 
    Question AS q
UNION ALL
SELECT 
    a.Id AS Id,
    q.UserId AS UserId,
    a.Text AS Text,
    'Answer' AS Source,
    q.ConversationId AS ConversationId,
    DATEDIFF(SECOND, '19700101', a.Date) AS Timestamp
FROM 
    Answer AS a
JOIN
    Question AS q ON a.QuestionId = q.Id;


");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP VIEW IF EXISTS QuestionAnswer");
        }
    }
}
