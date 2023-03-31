using WarGamesAPI.Interfaces;
using WarGamesAPI.Model;

#pragma warning disable CS1998

namespace WarGamesAPI.Services;

public class GptService : IGptService
{
    readonly IQuestionRepository _questionRepo;

    public GptService(IQuestionRepository questionRepo)
    {
        _questionRepo = questionRepo;
    }

    public async Task<Answer?> AskQuestion(Question question)
    {
        var answer = await GenerateAnswer();

        return new Answer
        {
            QuestionId = question.Id, 
            Text = answer, Time = DateTime.Now
        };

    }


    private async Task<string> GenerateAnswer()
    {
        return "42";
    }
}