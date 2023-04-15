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

    public QuestionController(ILogger<QuestionController> logger, IGptService gptService, 
        IQuestionRepository questionRepo, IMapper mapper)
    {
        _logger = logger;
        _gptService = gptService;
        _questionRepo = questionRepo;
        _mapper = mapper;
    }

    [ValidateToken]
    [HttpPost("askquestion")]
    public async Task<ActionResult<AnswerDto>> AskQuestion(AskQuestionDto userQuestion)
    {
        if (!Request.Headers.ContainsKey("Authorization") || string.IsNullOrEmpty(Request.Headers["Authorization"])) 
            return BadRequest("The Authorization header is required.");

        var question = _mapper.Map<QuestionDto>(userQuestion);

        var mockReply = userQuestion.MockReply;
        
        question.UserId = TokenData.getUserId(Request.Headers["Authorization"]!);

        
        //_logger.LogInformation($"AskQuestion called. userId: {question.UserId} Question: {question.QuestionText}.");

        if (string.IsNullOrEmpty(question.Text))
        {
            return BadRequest("The text of the user question is required.");
        }

        if (question.UserId == 0)
        {
            return BadRequest("Faulty userId");
        }

        if (question.ConversationId != 0)
        {
            
            if (!await _questionRepo.ConversationExists(question.ConversationId))
            {
                return BadRequest($"There is no conversation with Id {question.ConversationId}");
            }

            if (await _questionRepo.GetConversationUserId(question.ConversationId) != question.UserId)
            {
                return BadRequest("No such conversationId for this user");
            }

        }

        
        try
        {
            
            var savedQuestion = await _questionRepo.SaveQuestionAsync(question);

            if (savedQuestion is null) return StatusCode(500);
            savedQuestion.MockReply = mockReply;
            var answer = await _gptService.AskQuestion(savedQuestion);

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
    [HttpGet("getuserquestions")]
    public async Task<ActionResult<List<QuestionDto>>> GetUserQuestions()
    {
        if (!Request.Headers.ContainsKey("Authorization") || string.IsNullOrEmpty(Request.Headers["Authorization"])) 
            return BadRequest("The Authorization header is required.");
        var userId = TokenData.getUserId(Request.Headers["Authorization"]!);

        //_logger.LogInformation("GetMessages called");

        var questions = await _questionRepo.GetUserQuestionsAsync(userId);
        return questions.Any() ? Ok(questions) : NotFound();
    }

    [ValidateToken]
    [HttpGet("getquestion/{questionId}")]
    public async Task<ActionResult<QuestionDto>> GetQuestion(int questionId)
    {
        //_logger.LogInformation($"GetQuestion called. QuestionId: {questionId}");
        var question = await _questionRepo.GetQuestionAsync(questionId);
        return question is null ? NotFound() : Ok(question);
        
    }

    [ValidateToken]
    [HttpGet("getanswer/{answerId}")]
    public async Task<ActionResult<AnswerDto>> GetAnswer(int answerId)
    {
        var answer = await _questionRepo.GetAnswerAsync(answerId);
        return answer is null ? NotFound() : Ok(answer);
    }

    [ValidateToken]
    [HttpGet("getanswers/{questionId}")]
    public async Task<ActionResult<List<AnswerDto>>> GetAnswers(int questionId)
    {
        var answers = await _questionRepo.GetAnswersAsync(questionId);
        return answers.Any() ? Ok(answers) : NotFound();
    }

    [ValidateToken]
    [HttpGet("getconversation/{conversationId}")]
    public async Task<ActionResult<ConversationDto>> GetConversation(int conversationId)
    {
        var conversation = await _questionRepo.GetConversationAsync(conversationId);
        return conversation is null ? NotFound() : Ok(conversation);
    }

    [ValidateToken]
    [HttpGet("getconversations")]
    public async Task<ActionResult<List<ConversationDto>>> GetConversations()
    {
        if (!Request.Headers.ContainsKey("Authorization") || string.IsNullOrEmpty(Request.Headers["Authorization"])) 
            return BadRequest("The Authorization header is required.");
        var userId = TokenData.getUserId(Request.Headers["Authorization"]!);

        var conversations = await _questionRepo.GetConversationsAsync(userId);
        return conversations.Count == 0 ? NotFound() : Ok(conversations);
    }

    [ValidateToken]
    [HttpDelete("deletequestion/{questionId}")]
    public async Task<IActionResult> DeleteQuestion(int questionId)
    {

        try
        {
            var question = await _questionRepo.GetQuestionAsync(questionId);
            if (question == null)
            {
                return NotFound(new ResponseMessageDto { Message = $"Question with id {questionId} not found" });
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
    [HttpDelete("deleteanswer/{answerId}")]
    public async Task<IActionResult> DeleteAnswer(int answerId)
    {

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
    [HttpDelete("deleteconversation/{conversationId}")]
    public async Task<IActionResult> DeleteConversation(int conversationId)
    {

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