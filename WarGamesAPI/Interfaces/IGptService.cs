using WarGamesAPI.Model;

namespace WarGamesAPI.Interfaces;

public interface IGptService
{
    Task<Answer?> AskQuestion(Question question);
}