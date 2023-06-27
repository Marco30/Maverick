using WarGamesAPI.DTO;
using WarGamesAPI.Model;

namespace WarGamesAPI.Interfaces;

public interface IChatHistoryRepository
{

    Task<List<QuestionDto>> GetUserQuestionsAsync(int userId);
    Task<QuestionDto?> GetQuestionAsync(int questionId);
    Task<AnswerDto?> GetAnswerAsync(int answerId);
    Task<ConversationDto?> GetConversationDtoAsync(int conversationId);
    Task<List<AnswerDto>> GetAnswersAsync(int questionId);
    Task<List<ConversationInfoDto>> GetConversationInfosAsync(int userId);
    Task<string?> ChangeConversationNameAsync(int conversationId, string newName);
    Task<Conversation?> CreateConversationAsync(int userId, string conversationName);
    Task<SearchResultDto> SearchConversationHistoryAsync(int userId, string searchText);
    Task<List<QuestionAnswer>> GetQuestionAnswersAsync();
    Task<Conversation?> GetConversationAsync(int conversationId);
    


    Task DeleteQuestionAsync(int questionId);
    Task DeleteAnswerAsync(int answerId);
    Task DeleteConversationAsync(int conversationId);
    Task<List<QuestionDto>> GetQuestionsFromConversationAsync(int conversationId);
    Task<List<AnswerDto>> GetAnswersFromConversationAsync(int conversationId);

    Task<Answer?> SaveQuestionAndAnswerAsync(QuestionDto userQuestion, AnswerDto answer, bool mockReply);

    Task UpdateConversationAsync(int conversationId);
}



