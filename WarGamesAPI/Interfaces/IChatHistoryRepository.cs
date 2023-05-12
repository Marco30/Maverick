using WarGamesAPI.DTO;
using WarGamesAPI.Model;

namespace WarGamesAPI.Interfaces;

public interface IChatHistoryRepository
{

    Task<List<QuestionDto>> GetUserQuestionsAsync(int userId);
    Task<QuestionDto?> GetQuestionAsync(int questionId);
    Task<AnswerDto?> GetAnswerAsync(int answerId);
    Task<ConversationDto?> GetConversationAsync(int conversationId);
    Task<List<AnswerDto>> GetAnswersAsync(int questionId);
    Task<List<ConversationInfoDto>> GetConversationInfosAsync(int userId);
    Task<string?> ChangeConversationNameAsync(int conversationId, string newName);
    Task<Conversation?> CreateConversationAsync(int userId, string conversationName);
    


    Task DeleteQuestionAsync(int questionId);
    Task DeleteAnswerAsync(int answerId);
    Task DeleteConversationAsync(int conversationId);
    Task<List<QuestionDto>> GetQuestionsFromConversationAsync(int conversationId);
    Task<List<AnswerDto>> GetAnswersFromConversationAsync(int conversationId);

    Task<Answer?> SaveQuestionAndAnswerAsync(QuestionDto userQuestion, AnswerDto answer);

    Task UpdateConversationAsync(int conversationId);
}



