using WarGamesAPI.Model;

namespace WarGamesAPI.Interfaces;

public interface IQuestionService
{
    Task<Answer?> AskQuestion(Question question);
}