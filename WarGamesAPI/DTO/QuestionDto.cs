namespace WarGamesAPI.DTO;

public class QuestionDto
{
    public int Id { get; set; }
    public int? UserId { get; set; }
    public int ConversationId { get; set; }
    public string? QuestionText { get; set; }
}