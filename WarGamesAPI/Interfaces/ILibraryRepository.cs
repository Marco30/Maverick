using Microsoft.EntityFrameworkCore;
using WarGamesAPI.DTO;
using WarGamesAPI.Model;

namespace WarGamesAPI.Interfaces;

public interface ILibraryRepository
{
    Task<List<ConversationInfoDto>> GetConversationInfosAsync(int userId);
    Task<ConversationDto?> GetConversationAsync(int conversationId);
    Task<LibraryConversation?> SaveQuestionAndAnswersToLibraryAsync(int questionId, int? libraryConversationId);
    Task<LibraryConversation?> SaveQuestionToLibraryAsync(int questionId, int? libraryConversationId);
    Task<LibraryConversation?> SaveAnswerToLibraryAsync(int answerId, int? libraryConversationId);
    Task DeleteLibraryConversationAsync(int conversationId);
    Task DeleteLibraryAnswerAsync(int answerId);
    Task DeleteLibraryQuestionAsync(int questionId);
    Task<string?> ChangeLibraryConversationNameAsync(int conversationId, string newName);
    Task<LibraryConversation?> CreateLibraryConversationAsync(int userId, string conversationName);
    Task<LibraryConversation?> CreateLibraryConversationFromChatHistoryAsync(int userId, string? newName, int originalConversationId);



}



