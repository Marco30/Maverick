using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace WarGamesAPI.DTO
{
    public class EmailRequestDto
    {
        public string? ToEmail { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }
        public List<IFormFile>? Attachments { get; set; }
    }
}
