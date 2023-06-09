namespace WarGamesAPI.DTO;

public class SearchDto
{
    public bool LocalSearch { get; set; } = true;
    public int ConversationId { get; set; } = 0;
    public string SearchText { get; set; } = string.Empty;
}