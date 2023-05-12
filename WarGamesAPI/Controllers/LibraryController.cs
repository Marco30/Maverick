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
            var newConversation =
                await _libraryRepo.CreateLibraryConversationAsync(userId, createConversation.ConversationName);
            return Ok(_mapper.Map<ConversationInfoDto>(newConversation));

        }
        catch (Exception e)
        {
            var error = new ResponseMessageDto { StatusCode = 500, Message = e.Message };
            return StatusCode(500, error);
        }

    }

    [ValidateToken]
    [HttpPost("saveaslibraryconversation")]
    public async Task<ActionResult<ConversationInfoDto>> SaveAsLibraryConversation(SaveAsLibraryConversationDto createConversation)
    {
        if (!Request.Headers.ContainsKey("Authorization") || string.IsNullOrEmpty(Request.Headers["Authorization"])) 
            return BadRequest("The Authorization header is required.");
        
        var userId = TokenData.getUserId(Request.Headers["Authorization"]!);

        if (!await _validationRepo.UserOwnsConversation(userId, createConversation.OriginalConversationId)) return NotFound();
        
        try
        {
            var newConversation = await _libraryRepo.CreateLibraryConversationFromChatHistoryAsync
                (userId, createConversation.NewConversationName, createConversation.OriginalConversationId);
            
            return Ok(_mapper.Map<ConversationInfoDto>(newConversation));

        }
        catch (Exception e)
        {
            var error = new ResponseMessageDto { StatusCode = 500, Message = e.Message };
            return StatusCode(500, error);
        }

    }

    [ValidateToken]
    [HttpPost("renamelibraryconversation")]
    public async Task<ActionResult<string?>> RenameLibraryConversation(NameConversationDto name)
    {
        if (!Request.Headers.ContainsKey("Authorization") || string.IsNullOrEmpty(Request.Headers["Authorization"])) 
            return BadRequest("The Authorization header is required.");

        var userId = TokenData.getUserId(Request.Headers["Authorization"]!);

        if (name.NewName.IsNullOrEmpty()) return BadRequest("New name is required");

        if (!await _validationRepo.UserOwnsLibraryConversation(userId, name.ConversationId)) return NotFound();

        try
        {
            return Ok(await _libraryRepo.ChangeLibraryConversationNameAsync(name.ConversationId, name.NewName!));
        }
        catch (Exception e)
        {
            var error = new ResponseMessageDto { StatusCode = 500, Message = e.Message };
            return StatusCode(500, error);
        }
        
    }

    [ValidateToken]
    [HttpGet("getlibraryconversations")]
    public async Task<ActionResult<List<ConversationInfoDto>>> GetLibraryConversations()
    {
        if (!Request.Headers.ContainsKey("Authorization") || string.IsNullOrEmpty(Request.Headers["Authorization"])) 
            return BadRequest("The Authorization header is required.");
        var userId = TokenData.getUserId(Request.Headers["Authorization"]!);

        var conversations = await _libraryRepo.GetConversationInfosAsync(userId);
        return Ok(conversations);
    }

    [ValidateToken]
    [HttpPost("getlibraryconversation")]
    public async Task<ActionResult<ConversationDto>> GetLibraryConversation(GetConversationDto getConversation)
    {
        if (!Request.Headers.ContainsKey("Authorization") || string.IsNullOrEmpty(Request.Headers["Authorization"])) 
            return BadRequest("The Authorization header is required.");
        var userId = TokenData.getUserId(Request.Headers["Authorization"]!);

        if (!await _validationRepo.UserOwnsLibraryConversation(userId, getConversation.ConversationId)) return NotFound();

        var conversation = await _libraryRepo.GetConversationAsync(getConversation.ConversationId);

        return conversation is null ? NotFound() : Ok(conversation);

    }

    [ValidateToken]
    [HttpPost("savequestionandanswerstolibrary")]
    public async Task<ActionResult> SaveQuestionAndAnswersToLibrary(SaveQuestionDto saveQuestion)
    {
        if (!Request.Headers.ContainsKey("Authorization") || string.IsNullOrEmpty(Request.Headers["Authorization"]))
            return BadRequest("The Authorization header is required.");

        var userId = TokenData.getUserId(Request.Headers["Authorization"]!);

        if (saveQuestion.QuestionId != 0)
        {
            if (!await _validationRepo.UserOwnsQuestion(userId, saveQuestion.QuestionId))
                return BadRequest("Question not found");
        }

        try
        {
            await _libraryRepo
                .SaveQuestionAndAnswersToLibraryAsync(saveQuestion.QuestionId, saveQuestion.LibraryConversationId);

            return Ok();
        }
        catch (Exception e)
        {
            var error = new ResponseMessageDto { StatusCode = 500, Message = e.Message };
            return StatusCode(500, error);
        }

    }

    [ValidateToken]
    [HttpPost("saveanswertolibrary")]
    public async Task<ActionResult<ConversationInfoDto>> SaveAnswerToLibrary(SaveAnswerDto saveAnswer)
    {
        if (!Request.Headers.ContainsKey("Authorization") || string.IsNullOrEmpty(Request.Headers["Authorization"]))
            return BadRequest("The Authorization header is required.");

        var userId = TokenData.getUserId(Request.Headers["Authorization"]!);

        if (saveAnswer.AnswerId != 0)
        {
            if (!await _validationRepo.UserOwnsAnswer(userId, saveAnswer.AnswerId))
                return BadRequest("Question not found");
        }

        try
        {
            var libraryConversation = await _libraryRepo
                .SaveAnswerToLibraryAsync(saveAnswer.AnswerId, saveAnswer.LibraryConversationId);

            return Ok(_mapper.Map<ConversationInfoDto>(libraryConversation));
        }
        catch (Exception e)
        {
            var error = new ResponseMessageDto { StatusCode = 500, Message = e.Message };
            return StatusCode(500, error);
        }

    }

    [ValidateToken]
    [HttpDelete("deletelibraryconversation")]
    public async Task<IActionResult> DeleteLibraryConversation(GetConversationDto deleteConversation)
    {
        if (!Request.Headers.ContainsKey("Authorization") || string.IsNullOrEmpty(Request.Headers["Authorization"])) 
            return BadRequest("The Authorization header is required.");
        var userId = TokenData.getUserId(Request.Headers["Authorization"]!);
        var conversationId = deleteConversation.ConversationId;

        if (!await _validationRepo.UserOwnsLibraryConversation(userId, conversationId)) return NotFound();

        try
        {
            
            await _libraryRepo.DeleteLibraryConversationAsync(conversationId);
            return NoContent();

        }
        catch (Exception e)
        {
            var error = new ResponseMessageDto { StatusCode = 500, Message = e.Message };
            return StatusCode(500, error);
        }

    }

    [ValidateToken]
    [HttpDelete("deletelibraryanswer")]
    public async Task<IActionResult> DeleteLibraryAnswer(GetAnswerDto deleteAnswer)
    {
        if (!Request.Headers.ContainsKey("Authorization") || string.IsNullOrEmpty(Request.Headers["Authorization"])) 
            return BadRequest("The Authorization header is required.");
        var userId = TokenData.getUserId(Request.Headers["Authorization"]!);
        var answerId = deleteAnswer.AnswerId;

        if (!await _validationRepo.UserOwnsLibraryAnswer(userId, answerId)) return NotFound();

        try
        {
            
            await _libraryRepo.DeleteLibraryAnswerAsync(answerId);
            return NoContent();

        }
        catch (Exception e)
        {
            var error = new ResponseMessageDto { StatusCode = 500, Message = e.Message };
            return StatusCode(500, error);
        }


    }

    [ValidateToken]
    [HttpDelete("deletelibraryquestion")]
    public async Task<IActionResult> DeleteLibraryQuestion(GetQuestionDto deleteQuestion)
    {
        if (!Request.Headers.ContainsKey("Authorization") || string.IsNullOrEmpty(Request.Headers["Authorization"])) 
            return BadRequest("The Authorization header is required.");
        var userId = TokenData.getUserId(Request.Headers["Authorization"]!);
        var questionId = deleteQuestion.QuestionId;

        if (!await _validationRepo.UserOwnsLibraryQuestion(userId, questionId)) return NotFound();

        try
        {
            await _libraryRepo.DeleteLibraryQuestionAsync(questionId);
            return NoContent();

        }
        catch (Exception e)
        {
            var error = new ResponseMessageDto { StatusCode = 500, Message = e.Message };
            return StatusCode(500, error);
        }
        
    }


}