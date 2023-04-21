public class GptApiException : Exception
{
    public GptApiException(string? message) : base(message ?? "An unspecified API error occurred")
    {
    }
}