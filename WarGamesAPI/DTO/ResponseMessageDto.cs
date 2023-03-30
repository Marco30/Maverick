namespace WarGamesAPI.DTO;

public class ResponseMessageDto
{
    public bool Error { get; set; }
    public int StatusCode { get; set; }
    public string? Message { get; set; }
}