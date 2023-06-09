using Microsoft.AspNetCore.Mvc;
using MiracleMileAPI.Sessions;
using Typesense;
using WarGamesAPI.DTO;
using WarGamesAPI.Filters;
using WarGamesAPI.Interfaces;
using static System.String;

namespace WarGamesAPI.Controllers;

[Route("wargames")]
[ApiController]
public class SearchController : ControllerBase
{
    readonly ILogger<SearchController> _logger;
    readonly ISearchService _searchService;
    readonly IChatHistoryRepository _chatHistoryRepo;

    public SearchController(ILogger<SearchController> logger, ISearchService searchService,
        IChatHistoryRepository chatHistoryRepo)
    {
        _logger = logger;
        _searchService = searchService;
        _chatHistoryRepo = chatHistoryRepo;
    }

    [ValidateToken]
    [HttpPost("search")]
    public async Task<ActionResult<SearchResultDto>> SearchConversations(SearchDto search)
    {
        if (!Request.Headers.ContainsKey("Authorization") || IsNullOrEmpty(Request.Headers["Authorization"]))
            return BadRequest("The Authorization header is required.");

        if (IsNullOrEmpty(search.SearchText))
            return BadRequest("The search text is required.");

        var userId = TokenData.getUserId(Request.Headers["Authorization"]!);

        try
        {
            SearchResultDto result;

            if (!search.LocalSearch)
            {
                result = await _searchService.SearchConversationHistoryAsync(userId, search.SearchText);
            }
            else
            {
                result = await _chatHistoryRepo.SearchConversationHistoryAsync(userId, search.SearchText);
            }

            return Ok(result);
        }
        catch (Exception e)
        {
            var error = new ResponseMessageDto { StatusCode = 500, Message = e.Message };
            return StatusCode(500, error);
        }

    }

    [ValidateToken]
    [HttpPost("highlight")]
    public async Task<ActionResult<ConversationDto>> SearchConversation(SearchDto search)
    {
        if (!Request.Headers.ContainsKey("Authorization") || IsNullOrEmpty(Request.Headers["Authorization"]))
            return BadRequest("The Authorization header is required.");

        if (IsNullOrEmpty(search.SearchText))
            return BadRequest("The search text is required.");
        if (search.ConversationId == 0)
            return BadRequest("Conversation Id is required.");

        var userId = TokenData.getUserId(Request.Headers["Authorization"]!);

        try
        {
            
            var result = await _searchService.SearchConversationAsync(userId, search.SearchText, search.ConversationId);
            return Ok(result);
        }
        catch (Exception e)
        {
            var error = new ResponseMessageDto { StatusCode = 500, Message = e.Message };
            return StatusCode(500, error);
        }

    }


    //[ValidateToken]
    //[HttpPost("autocompletesearch")]
    //public async Task<ActionResult<AutoCompleteResultDto>> AutocompleteSearch(SearchDto search)
    //{
    //    if (!Request.Headers.ContainsKey("Authorization") || IsNullOrEmpty(Request.Headers["Authorization"])) 
    //        return BadRequest("The Authorization header is required.");

    //    if (IsNullOrEmpty(search.SearchText)) 
    //        return BadRequest("The search text is required.");

    //    var userId = TokenData.getUserId(Request.Headers["Authorization"]!);

    //    try
    //    {
    //        var result = await _searchService.AutocompleteAsync(userId, search.SearchText);
    //        return Ok(result);
    //    }
    //    catch (Exception e)
    //    {
    //        var error = new ResponseMessageDto { StatusCode = 500, Message = e.Message };
    //        return StatusCode(500, error);
    //    }

    //}




}