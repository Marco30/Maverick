using Microsoft.AspNetCore.Mvc;
using WarGamesAPI.DTO;
using WarGamesAPI.Model;
#pragma warning disable CS1998

namespace WarGamesAPI.Controllers;

[Route("wargames")] [ApiController]
public class MessageController : ControllerBase
{

[HttpGet("getmessages")]
public async Task<ActionResult<List<Message>>> GetMessages()
{
    var random = new Random();
    var result = new List<Message>
    {
        new() { Id = random.Next(), Question = "What is 2 + 2?", Answer = "5" },
        new() { Id = random.Next(), Question = "Is the Easter Bunny real?", Answer = "Yes" },
        new() { Id = random.Next(), Question = "What is the capital of France?", Answer = "Paris" },
        new() { Id = random.Next(), Question = "What is the largest planet in our solar system?", Answer = "Jupiter" },
        new() { Id = random.Next(), Question = "Who wrote 'To Kill a Mockingbird'?", Answer = "Harper Lee" },
        new() { Id = random.Next(), Question = "What is the chemical symbol for oxygen?", Answer = "O" },
        new() { Id = random.Next(), Question = "What year was the first iPhone released?", Answer = "2007" },
        new() { Id = random.Next(), Question = "What is the smallest prime number?", Answer = "2" },
        new() { Id = random.Next(), Question = "What is the speed of light in a vacuum?", Answer = "299,792 km/s" },
        new() { Id = random.Next(), Question = "Who painted the Mona Lisa?", Answer = "Leonardo da Vinci" },
        new() { Id = random.Next(), Question = "What is the square root of 16?", Answer = "4" },
        new() { Id = random.Next(), Question = "What does HTTP stand for?", Answer = "HyperText Transfer Protocol" },
        new() { Id = random.Next(), Question = "What is the tallest mountain in the world?", Answer = "Mount Everest" },
        new() { Id = random.Next(), Question = "What is the most widely spoken language?", Answer = "English" },
        new() { Id = random.Next(), Question = "What is the world's smallest country?", Answer = "Vatican City" },
        new() { Id = random.Next(), Question = "Which animal is the fastest on land?", Answer = "Cheetah" },
        new() { Id = random.Next(), Question = "Which country has the largest population?", Answer = "China" },
        new() { Id = random.Next(), Question = "What is the primary source of energy for Earth?", Answer = "The Sun" },
        new() { Id = random.Next(), Question = "Which element is most abundant in the Earth's crust?", Answer = "Oxygen" },
        new() { Id = random.Next(), Question = "What is the distance between the Earth and the Moon?", Answer = "384,400 km" }
    };

    return result;
}

    [HttpGet("getmessage/{id}")]
    public async Task<ActionResult<Message>> Get(int id)
    {
        var message = new Message { Id = id, Question = "What is 2 + 2?", 
            Answer = $"This is the answer to the question with id {id}" };

        return Ok(message);
    }

    [HttpPost("askquestion")]
    public async Task<ActionResult<Message>> AskQuestion(QuestionDto question)
    {
        var message = new Message { Id = new Random().Next(), Question = question.Question, Answer = "42" };
        return StatusCode(201, message);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        return NoContent();
    }
}