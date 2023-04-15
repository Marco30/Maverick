using WarGamesAPI.DTO;
using WarGamesAPI.Interfaces;

#pragma warning disable CS1998

namespace WarGamesAPI.Services;

public class GptService : IGptService
{
    readonly ILogger<GptService> _logger;

    public GptService(ILogger<GptService> logger)
    {
        _logger = logger;
    }

    public async Task<AnswerDto?> AskQuestion(QuestionDto question)
    {
        var answer = await GenerateAnswer();

        return new AnswerDto
        {
            QuestionId = question.Id, 
            Text = answer, Date = DateTime.Now
        };

    }


    private async Task<string> GenerateAnswer()
    {
        return "42";
    }
}