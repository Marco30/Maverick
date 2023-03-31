using Microsoft.AspNetCore.Mvc;
using MiracleMileAPI.Sessions;
using WarGamesAPI.DTO;
using WarGamesAPI.Filters;
using WarGamesAPI.Interfaces;
using WarGamesAPI.Model;

#pragma warning disable CS1998

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

    [ValidateToken]
    [HttpGet("getuserquestions")]
    public async Task<ActionResult<List<QuestionDto>>> GetUserQuestions()
    {
        var userId = TokenData.getUserId(Request.Headers["Authorization"]);

        _logger.LogInformation("GetMessages called");

        return Ok(await _questionRepo.GetUserQuestions(userId));
    }

    [ValidateToken]
    [HttpGet("getquestion/{id}")]
    public async Task<ActionResult<QuestionDto>> GetQuestion(int id)
    {
        _logger.LogInformation($"GetQuestion called. QuestionId: {id}");

        var question = await _questionRepo.GetQuestion(id);
        if (question != null)
        {
            return Ok(question);
        }

        return NotFound();
    }

    [ValidateToken]
    [HttpGet("getanswer/{answerId}")]
    public async Task<ActionResult<AnswerDto>> GetAnswer(int answerId)
    {

        var answer = await _questionRepo.GetAnswer(answerId);
        if (answer != null)
        {
            return Ok(answer);
        }

        return NotFound();
    }

    [ValidateToken]
    [HttpGet("getanswers/{questionId}")]
    public async Task<ActionResult<List<AnswerDto>>> GetAnswers(int questionId)
    {

        var answer = await _questionRepo.GetAnswers(questionId);
        if (answer != null)
        {
            return Ok(answer);
        }

        return NotFound();
    }

    [ValidateToken]
    [HttpPost("askquestion")]
    public async Task<ActionResult<AnswerDto>> AskQuestion(AskQuestionDto userQuestion)
    {
        userQuestion.UserId = TokenData.getUserId(Request.Headers["Authorization"]);

        _logger.LogInformation($"AskQuestion called. userId: {userQuestion.UserId} Question: {userQuestion.Text}.");
        var question = await _questionRepo.SaveQuestion(userQuestion);
        var answer = new Answer();

        if (question != null)
        {
            answer = await _gptService.AskQuestion(question);
        }

        if (answer is null)
        {
            return StatusCode(500);
        }
        
        var result = await _questionRepo.SaveAnswer(answer);
        
        return StatusCode(201, result);
    }

    [ValidateToken]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteQuestion(int questionId)
    {
        await _questionRepo.DeleteQuestion(questionId);
        return NoContent();
    }
}