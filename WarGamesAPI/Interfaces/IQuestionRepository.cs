using WarGamesAPI.DTO;
using WarGamesAPI.Model;

namespace WarGamesAPI.Interfaces;

public interface IQuestionRepository
{
    Task<Question?> SaveQuestion(AskQuestionDto userQuestion);
    Task<Answer?> SaveAnswer(Answer answer);
    Task<List<QuestionDto>> GetUserQuestions(int userId);
    Task<QuestionDto?> GetQuestion(int questionId);




}



