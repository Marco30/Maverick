namespace WarGamesAPI.DTO;

public class ConversationInfoDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public DateTime Date { get; set; }
    public DateTime Updated { get; set; }
    public int UserId { get; set; }
}