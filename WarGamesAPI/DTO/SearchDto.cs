namespace WarGamesAPI.DTO;

public class SearchDto
{
    public bool LocalSearch { get; set; } = true;
    public string SearchText { get; set; } = string.Empty;
}