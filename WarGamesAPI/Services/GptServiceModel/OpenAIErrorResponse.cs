using System.Text.Json.Serialization;
using WarGamesAPI.Services;

public class OpenAIErrorResponse
{
    [JsonPropertyName("error")]
    public OpenAIError? Error { get; set; }
}