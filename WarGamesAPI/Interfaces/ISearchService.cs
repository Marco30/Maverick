using WarGamesAPI.DTO;
using WarGamesAPI.Model;

namespace WarGamesAPI.Interfaces;

public interface ISearchService
{
    Task<SearchResultDto> SearchConversationHistoryAsync(int userId, string searchText);
    Task IndexAllDocuments();
    Task<ConversationDto> SearchConversationAsync(int userId, string searchText, int conversationId);
    Task IndexNewDocumentsAsync(List<QuestionAnswer> document);


}



