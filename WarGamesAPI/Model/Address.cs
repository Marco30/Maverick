using System.ComponentModel.DataAnnotations;

namespace WarGamesAPI.Model;

public class Address
{
    [Key]
    public int UserId { get; set; }

    public string? City { get; set; }
    public string? Country { get; set; }
    public string? Street { get; set; }
    public string? ZipCode { get; set; }
    public string? Attention { get; set; }
    public string? Municipality { get; set; }
    public string? CareOf { get; set; }

    public User? User { get; set; } 
}


