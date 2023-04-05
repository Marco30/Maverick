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

    public QuestionController(ILogger<QuestionController> logger, IGptService gptService, 
        IQuestionRepository questionRepo)
    {
        _logger = logger;
        _gptService = gptService;
        _questionRepo = questionRepo;
    }

    //[ValidateToken]
    [HttpPost("askquestion")]
    public async Task<ActionResult<AnswerDto>> AskQuestion(AskQuestionDto userQuestion)
    {
        //if (!Request.Headers.ContainsKey("Authorization") || string.IsNullOrEmpty(Request.Headers["Authorization"])) 
        //    return BadRequest("The Authorization header is required.");
        //userQuestion.UserId = TokenData.getUserId(Request.Headers["Authorization"]!);

        userQuestion.UserId = 5;
        
        _logger.LogInformation($"AskQuestion called. userId: {userQuestion.UserId} Question: {userQuestion.Text}.");

        if (string.IsNullOrEmpty(userQuestion.Text))
        {
            return BadRequest("The text of the user question is required.");
        }

        if (userQuestion.UserId == 0)
        {
            return BadRequest("Faulty userId");
        }
        
        
        try
        {
            
            var question = await _questionRepo.SaveQuestionAsync(userQuestion);
            
            var answer = new AnswerDto();

            if (question != null) answer = await _gptService.AskQuestion(question);

            if (answer is null) return StatusCode(500);
            
            answer.ConversationId = userQuestion.ConversationId;

        
            var result = await _questionRepo.SaveAnswerAsync(answer);
        
            return StatusCode(201, result);

        }
        catch (Exception e)
        {
            var error = new ResponseMessageDto { StatusCode = 500, Message = e.Message };
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

        _logger.LogInformation("GetMessages called");

        var questions = await _questionRepo.GetUserQuestionsAsync(userId);
        return questions.Any() ? Ok(questions) : NotFound();
    }

    [ValidateToken]
    [HttpGet("getquestion/{questionId}")]
    public async Task<ActionResult<QuestionDto>> GetQuestion(int questionId)
    {
        _logger.LogInformation($"GetQuestion called. QuestionId: {questionId}");
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
    [HttpDelete("deletequestion/{questionId}")]
    public async Task<IActionResult> DeleteQuestion(int questionId)
    {

        try
        {
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
            var conversation = await _questionRepo.GetAnswerAsync(conversationId);
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