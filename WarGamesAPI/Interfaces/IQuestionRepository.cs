using WarGamesAPI.DTO;
using WarGamesAPI.Model;

namespace WarGamesAPI.Interfaces;

public interface IQuestionRepository
{
    Task<Question?> SaveQuestion(AskQuestionDto userQuestion);
    Task<AnswerDto?> SaveAnswer(Answer answer);
    Task<List<QuestionDto>> GetUserQuestions(int userId);
    Task<QuestionDto?> GetQuestion(int questionId);
    Task DeleteQuestion(int questionId);
    Task<AnswerDto?> GetAnswer(int answerId);
    Task<List<AnswerDto>> GetAnswers(int questionId);




}



