﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MiracleMileAPI.Sessions;
using WarGamesAPI.DTO;
using WarGamesAPI.Filters;
using WarGamesAPI.Interfaces;

namespace WarGamesAPI.Controllers;

[Route("wargames")]
[ApiController]
public class ConversationController : ControllerBase
{
    readonly ILogger<ConversationController> _logger;
    readonly IGptService _gptService;
    readonly IChatHistoryRepository _questionRepo;
    readonly IMapper _mapper;
    readonly IValidationRepository _validationRepo;

    public ConversationController(ILogger<ConversationController> logger, IGptService gptService, 
        IChatHistoryRepository questionRepo, IMapper mapper, IValidationRepository validationRepo)
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
    [HttpPost("createconversation")]
    public async Task<ActionResult<ConversationInfoDto>> CreateConversation(CreateConversationDto createConversation)
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
            var newConversation = await _questionRepo.CreateConversationAsync(userId, createConversation.ConversationName);
            return Ok(_mapper.Map<ConversationInfoDto>(newConversation));

        }
        catch (Exception e)
        {
            var error = new ResponseMessageDto { StatusCode = 500, Message = e.Message };
            return StatusCode(500, error);
        }

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
            
            var answer = await _gptService.AskQuestion(question, userQuestion.MockReply);

            if (answer is null) return StatusCode(500);

            var savedAnswer = await _questionRepo.SaveQuestionAndAnswerAsync(question, answer);

            await _questionRepo.UpdateConversationAsync(question.ConversationId);
            
            if (savedAnswer is null) return StatusCode(500);

            
            return StatusCode(201, _mapper.Map<AnswerDto>(savedAnswer));
            


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
    [HttpPost("changeconversationname")]
    public async Task<ActionResult<string?>> ChangeConversationName(NameConversationDto name)
    {
        if (!Request.Headers.ContainsKey("Authorization") || string.IsNullOrEmpty(Request.Headers["Authorization"])) 
            return BadRequest("The Authorization header is required.");
        var userId = TokenData.getUserId(Request.Headers["Authorization"]!);

        if (!await _validationRepo.UserOwnsConversation(userId, name.ConversationId)) return NotFound();

        try
        {
            await _questionRepo.ChangeConversationNameAsync(name.ConversationId, name.NewName);
            var obj = new
            {
                Info = "name has changed"
            };
            return Ok(obj);
        }
        catch (Exception e)
        {
            var error = new ResponseMessageDto { StatusCode = 500, Message = e.Message };
            return StatusCode(500, error);
        }
        
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
    public async Task<IActionResult> DeleteAnswer([FromBody]GetAnswerDto deleteAnswer)
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
    public async Task<IActionResult> DeleteConversation([FromBody] GetConversationDto deleteConversation)
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
            var obj = new
            {
                Info = "Conversation is deleted"
            };
            return Ok(obj);

        }
        catch (Exception e)
        {
            var error = new ResponseMessageDto { StatusCode = 500, Message = e.Message };
            return StatusCode(500, error);
        }


    }


}