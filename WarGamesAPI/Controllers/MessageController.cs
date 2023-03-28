using Microsoft.AspNetCore.Mvc;
using WarGamesAPI.Model;

namespace WarGamesAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MessageController : ControllerBase
{
    // GET: api/<ValuesController>
    [HttpGet]
    public IEnumerable<Message> GetMessages()
    {
        var result = new List<Message>
        {
            new() { Id = 1, Question = "What is 2 + 2?", Answer = "5" },
            new() { Id = 2, Question = "Is the Easter Bunny real?", Answer = "Yes" }
        };

        return result;
    }

    // GET api/<ValuesController>/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
        return "value";
    }

    // POST api/<ValuesController>
    [HttpPost]
    public void Post([FromBody] string value)
    {
    }

    // PUT api/<ValuesController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/<ValuesController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}