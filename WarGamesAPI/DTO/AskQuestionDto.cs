namespace WarGamesAPI.DTO;

public class AskQuestionDto
{
    public int UserId { get; set; } = 0;
    public int ConversationId { get; set; } = 0;
    public string? Text { get; set; }
}