using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MiracleMileAPI.Sessions;
using WarGamesAPI.DTO;
using WarGamesAPI.Filters;
using WarGamesAPI.Interfaces;

namespace WarGamesAPI.Controllers;

[Route("wargames")]
[ApiController]
public class LibraryController : ControllerBase
{
    readonly ILogger<LibraryController> _logger;
    readonly ILibraryRepository _libraryRepo;
    readonly IMapper _mapper;
    readonly IValidationRepository _validationRepo;

    public LibraryController(ILogger<LibraryController> logger, ILibraryRepository libraryRepo,
        IMapper mapper, IValidationRepository validationRepo)
    {
        _logger = logger;
        _libraryRepo = libraryRepo;
        _mapper = mapper;
        _validationRepo = validationRepo;
    }

    [ValidateToken]
    [HttpPost("createlibraryconversation")]
    public async Task<ActionResult<ConversationInfoDto>> CreateLibraryConversation(CreateConversationDto createConversation)
    {
        if (!Request.Headers.ContainsKey("Authorization") || string.IsNullOrEmpty(Request.Headers["Authorization"])) 
            return BadRequest("The Authorization header is required.");
        
        var userId = TokenData.getUserId(Request.Headers["Authorization"]!);

        if (string.IsNullOrEmpty(createConversation.ConversationName?.Trim()))
        {
            return BadRequest("Conversation name is required");
        }
        
        try
        {
            var newConversation = await _libraryRepo
                .CreateLibraryConversationAsync(userId, createConversation.ConversationName);
            return Ok(_mapper.Map<ConversationInfoDto>(newConversation));

        }
        catch (Exception e)
        {
            var error = new ResponseMessageDto { StatusCode = 500, Message = e.Message };
            return StatusCode(500, error);
        }

    }

    
}