using WarGamesAPI.DTO;

namespace WarGamesAPI.Interfaces;

public interface ISearchService
{


    Task<SearchResultDto> SearchConversationHistoryAsync(int userId, string searchText);



}



