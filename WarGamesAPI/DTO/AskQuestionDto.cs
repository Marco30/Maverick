namespace WarGamesAPI.DTO;

public class AskQuestionDto
{
    public int? ConversationId { get; set; } = 0;
    public string? QuestionText { get; set; }
}