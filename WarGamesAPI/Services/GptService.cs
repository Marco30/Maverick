using WarGamesAPI.DTO;
using WarGamesAPI.Interfaces;

#pragma warning disable CS1998

namespace WarGamesAPI.Services;

public class GptService : IGptService
{
    readonly ILogger<GptService> _logger;
    readonly string? _apiKey;

    public GptService(ILogger<GptService> logger, IConfiguration configuration)
    {
        _logger = logger;
        _apiKey = configuration["openAiKey"];
    }

    public async Task<AnswerDto?> AskQuestion(QuestionDto question)
    {
        var answer = await GenerateAnswer();

        return new AnswerDto
        {
            QuestionId = question.Id, 
            AnswerText = answer, Time = DateTime.Now
        };

    }


    private async Task<string> GenerateAnswer()
    {
        return "42";
    }
}