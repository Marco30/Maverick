using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MiracleMileAPI.Sessions;
using WarGamesAPI.DTO;
using WarGamesAPI.Filters;
using WarGamesAPI.Interfaces;

namespace WarGamesAPI.Controllers;

[Route("wargames")]
[ApiController]
public class QuestionController : ControllerBase
{
    readonly ILogger<QuestionController> _logger;
    readonly IGptService _gptService;
    readonly IQuestionRepository _questionRepo;
    readonly IMapper _mapper;
    readonly IValidationRepository _validationRepo;

    public QuestionController(ILogger<QuestionController> logger, IGptService gptService, 
        IQuestionRepository questionRepo, IMapper mapper, IValidationRepository validationRepo)
    {
        _logger = logger;
        _gptService = gptService;
        _questionRepo = questionRepo;
        _mapper = mapper;
        _validationRepo = validationRepo;
    }

    
    [ValidateToken]
    [HttpGet("getquestions")]
    public async Task<ActionResult<List<QuestionDto>>> GetQuestions()
    {
        if (!Request.Headers.ContainsKey("Authorization") || string.IsNullOrEmpty(Request.Headers["Authorization"])) 
            return BadRequest("The Authorization header is required.");
        var userId = TokenData.getUserId(Request.Headers["Authorization"]!);

        _logger.LogInformation("GetQuestions called");
        
        return Ok (await _questionRepo.GetUserQuestionsAsync(userId));
    }

    [ValidateToken]
    [HttpGet("getconversations")]
    public async Task<ActionResult<List<ConversationInfoDto>>> GetConversations()
    {
        if (!Request.Headers.ContainsKey("Authorization") || string.IsNullOrEmpty(Request.Headers["Authorization"])) 
            return BadRequest("The Authorization header is required.");
        var userId = TokenData.getUserId(Request.Headers["Authorization"]!);

        var conversations = await _questionRepo.GetConversationInfosAsync(userId);
        return Ok(conversations);
    }

    [ValidateToken]
    [HttpPost("askquestion")]
    public async Task<ActionResult<AnswerDto>> AskQuestion(AskQuestionDto userQuestion)
    {


        if (!Request.Headers.ContainsKey("Authorization") || string.IsNullOrEmpty(Request.Headers["Authorization"])) 
            return BadRequest("The Authorization header is required.");

        var userId = TokenData.getUserId(Request.Headers["Authorization"]!);

        var question = _mapper.Map<QuestionDto>(userQuestion);

        question.UserId = userId;

        
        if (string.IsNullOrEmpty(question.Text)) return BadRequest("The text of the user question is required.");

        if (question.ConversationId != 0)
        {
            if (!await _validationRepo.UserOwnsConversation(userId, question.ConversationId)) 
                return BadRequest("Conversation not found");
        }

        
        try
        {
            
            QuestionDto? savedQuestion = await _questionRepo.SaveQuestionAsync(question);

            if (savedQuestion is null) return StatusCode(500);
            AnswerDto? answer = await _gptService.AskQuestion(savedQuestion, userQuestion.MockReply);

            if (answer is null) return StatusCode(500);
            
            answer.ConversationId = savedQuestion.ConversationId;
            
            var result = await _questionRepo.SaveAnswerAsync(answer);
        
            return StatusCode(201, result);

        }
        catch (Exception e)
        {
            var error = new ResponseMessageDto { Error = true, StatusCode = 500, Message = $"{e.Message}" };
            return StatusCode(500, error);
        }

    }

    [ValidateToken]
    [HttpPost("getquestion")]
    public async Task<ActionResult<QuestionDto>> GetQuestion(GetQuestionDto getQuestion)
    {
        if (!Request.Headers.ContainsKey("Authorization") || string.IsNullOrEmpty(Request.Headers["Authorization"])) 
            return BadRequest("The Authorization header is required.");
        var userId = TokenData.getUserId(Request.Headers["Authorization"]!);

        if (!await _validationRepo.UserOwnsQuestion(userId, getQuestion.QuestionId)) return NotFound();

        var question = await _questionRepo.GetQuestionAsync(getQuestion.QuestionId);

        return question is null ? NotFound() : Ok(question);
        
    }

    [ValidateToken]
    [HttpPost("getanswer")]
    public async Task<ActionResult<AnswerDto>> GetAnswer(GetAnswerDto getAnswer)
    {
        if (!Request.Headers.ContainsKey("Authorization") || string.IsNullOrEmpty(Request.Headers["Authorization"])) 
            return BadRequest("The Authorization header is required.");
        var userId = TokenData.getUserId(Request.Headers["Authorization"]!);

        if (!await _validationRepo.UserOwnsAnswer(userId, getAnswer.AnswerId)) return NotFound();

        var answer = await _questionRepo.GetAnswerAsync(getAnswer.AnswerId);

        return answer is null ? NotFound() : Ok(answer);
        
    }

    [ValidateToken]
    [HttpPost("getanswers")]
    public async Task<ActionResult<List<AnswerDto>>> GetAnswers(GetAnswersDto getAnswers)
    {
        if (!Request.Headers.ContainsKey("Authorization") || string.IsNullOrEmpty(Request.Headers["Authorization"])) 
            return BadRequest("The Authorization header is required.");
        var userId = TokenData.getUserId(Request.Headers["Authorization"]!);

        if (!await _validationRepo.UserOwnsQuestion(userId, getAnswers.QuestionId)) return NotFound();

        var answers = await _questionRepo.GetAnswersAsync(getAnswers.QuestionId);
        
        return Ok(answers);
    }

    [ValidateToken]
    [HttpPost("getconversation")]
    public async Task<ActionResult<ConversationDto>> GetConversation(GetConversationDto getConversation)
    {
        if (!Request.Headers.ContainsKey("Authorization") || string.IsNullOrEmpty(Request.Headers["Authorization"])) 
            return BadRequest("The Authorization header is required.");
        var userId = TokenData.getUserId(Request.Headers["Authorization"]!);

        if (!await _validationRepo.UserOwnsConversation(userId, getConversation.ConversationId)) return NotFound();

        var conversation = await _questionRepo.GetConversationAsync(getConversation.ConversationId);

        return conversation is null ? NotFound() : Ok(conversation);

    }

    
    [ValidateToken]
    [HttpDelete("deletequestion")]
    public async Task<IActionResult> DeleteQuestion(GetQuestionDto deleteQuestion)
    {
        if (!Request.Headers.ContainsKey("Authorization") || string.IsNullOrEmpty(Request.Headers["Authorization"])) 
            return BadRequest("The Authorization header is required.");
        var userId = TokenData.getUserId(Request.Headers["Authorization"]!);
        var questionId = deleteQuestion.QuestionId;

        if (!await _validationRepo.UserOwnsQuestion(userId, questionId)) return NotFound();

        try
        {
            var question = await _questionRepo.GetQuestionAsync(questionId);
            if (question is null)
            {
                return NotFound(new ResponseMessageDto 
                    { Message = $"Question with id {questionId} not found" });
            }

            await _questionRepo.DeleteQuestionAsync(questionId);
            return NoContent();

        }
        catch (Exception e)
        {
            var error = new ResponseMessageDto { StatusCode = 500, Message = e.Message };
            return StatusCode(500, error);
        }


    }

    [ValidateToken]
    [HttpDelete("deleteanswer")]
    public async Task<IActionResult> DeleteAnswer(GetAnswerDto deleteAnswer)
    {
        if (!Request.Headers.ContainsKey("Authorization") || string.IsNullOrEmpty(Request.Headers["Authorization"])) 
            return BadRequest("The Authorization header is required.");
        var userId = TokenData.getUserId(Request.Headers["Authorization"]!);
        var answerId = deleteAnswer.AnswerId;

        if (!await _validationRepo.UserOwnsAnswer(userId, answerId)) return NotFound();
        
        try
        {
            var answer = await _questionRepo.GetAnswerAsync(answerId);
            if (answer == null)
            {
                return NotFound(new ResponseMessageDto { Message = $"Answer with id {answerId} not found" });
            }

            await _questionRepo.DeleteAnswerAsync(answerId);
            return NoContent();

        }
        catch (Exception e)
        {
            var error = new ResponseMessageDto { StatusCode = 500, Message = e.Message };
            return StatusCode(500, error);
        }


    }

    [ValidateToken]
    [HttpDelete("deleteconversation")]
    public async Task<IActionResult> DeleteConversation(GetConversationDto deleteConversation)
    {
        if (!Request.Headers.ContainsKey("Authorization") || string.IsNullOrEmpty(Request.Headers["Authorization"])) 
            return BadRequest("The Authorization header is required.");
        var userId = TokenData.getUserId(Request.Headers["Authorization"]!);
        var conversationId = deleteConversation.ConversationId;

        if (!await _validationRepo.UserOwnsConversation(userId, conversationId)) return NotFound();


        try
        {
            var conversation = await _questionRepo.GetConversationAsync(conversationId);
            if (conversation == null)
            {
                return NotFound(new ResponseMessageDto { Message = $"Conversation with id {conversationId} not found" });
            }

            await _questionRepo.DeleteConversationAsync(conversationId);
            return NoContent();

        }
        catch (Exception e)
        {
            var error = new ResponseMessageDto { StatusCode = 500, Message = e.Message };
            return StatusCode(500, error);
        }


    }
}