using System.ComponentModel.DataAnnotations.Schema;

namespace WarGamesAPI.Model;

public class Address
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string? City { get; set; }
    public string? Country { get; set; }
    public string? Street { get; set; }
    public string? ZipCode { get; set; }
    public string? Attention { get; set; }
    public string? Municipality { get; set; }
    public string? CareOf { get; set; }

    public int UserId { get; set; }
    public User? User { get; set; } 

}