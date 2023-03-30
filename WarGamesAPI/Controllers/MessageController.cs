using Microsoft.AspNetCore.Mvc;
using MiracleMileAPI.Sessions;
using WarGamesAPI.DTO;
using WarGamesAPI.Interfaces;
using WarGamesAPI.Model;

#pragma warning disable CS1998

namespace WarGamesAPI.Controllers;

[Route("wargames")]
[ApiController]
public class MessageController : ControllerBase
{
    readonly ILogger<MessageController> _logger;
    readonly IQuestionService _questionService;
    readonly IQuestionRepository _messageRepo;

    public MessageController(ILogger<MessageController> logger, IQuestionService questionService, 
        IQuestionRepository messageRepo)
    {
        _logger = logger;
        _questionService = questionService;
        _messageRepo = messageRepo;
    }


    [HttpGet("getuserquestions")]
    public async Task<ActionResult<List<QuestionDto>>> GetUserQuestions()
    {
        var userId = TokenData.getUserId(Request.Headers["Authorization"]);

        _logger.LogInformation("GetMessages called");

        return Ok(await _messageRepo.GetUserQuestions(userId));
    }

    [HttpGet("getquestion/{id}")]
    public async Task<ActionResult<QuestionDto>> Get(int id)
    {
        _logger.LogInformation($"GetQuestion called. QuestionId: {id}");

        var question = await _messageRepo.GetQuestion(id);
        if (question != null)
        {
            return Ok(question);
        }

        return NotFound();
    }

    [HttpPost("askquestion")]
    public async Task<ActionResult<Answer>> AskQuestion(AskQuestionDto userQuestion)
    {
        userQuestion.UserId = TokenData.getUserId(Request.Headers["Authorization"]);

        _logger.LogInformation($"AskQuestion called. userId: {userQuestion.UserId} Question: {userQuestion.Text}.");
        var question = await _messageRepo.SaveQuestion(userQuestion);
        var answer = new Answer();

        if (question != null)
        {
            answer = await _questionService.AskQuestion(question);
        }

        if (answer is null)
        {
            return StatusCode(500);
        }

        return StatusCode(201, answer);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteQuestion(int questionId)
    {
        return NoContent();
    }
}