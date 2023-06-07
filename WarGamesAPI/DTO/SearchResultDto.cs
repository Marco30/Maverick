namespace WarGamesAPI.DTO;

public class SearchResultDto
{
    public string SearchText { get; set; } = string.Empty;
    public HashSet<int> ConversationIds { get; set; } = new();
    public HashSet<int> QuestionIds { get; set; } = new();
    public HashSet<int> AnswerIds { get; set; } = new();
}