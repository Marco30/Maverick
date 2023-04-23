using System.Text.Json.Serialization;

public class Choice
{

    [JsonPropertyName("message")]
    public Message? Message { get; set; }

    [JsonPropertyName("index")]
    public int index { get; set; }

    [JsonPropertyName("finish_reason")]
    public string? finish_reason { get; set; }
}