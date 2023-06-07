using System.Text.Json.Serialization;

namespace WarGamesAPI.Services.SearchServiceModel;


public class QuestionAnswer
{
    [JsonPropertyName("id")]
    public int? Id { get; set; }

    [JsonPropertyName("text")]
    public string? Text { get; set; }

    [JsonPropertyName("userid")]
    public string? UserId { get; set; }

    [JsonPropertyName("conversationid")]
    public string? ConversationId { get; set; }

    [JsonPropertyName("source")]
    public string? Source { get; set; }

    [JsonPropertyName("timestamp")]
    public int Timestamp { get; set; } 
}
