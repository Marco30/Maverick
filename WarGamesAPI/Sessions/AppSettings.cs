namespace WarGamesAPI.Sessions;

public class AppSettings
{
    public int JWtTokenValidMinutes { get; set; }
    public int JWtTimeLeftReminderMinutes { get; set; }
    public string? JWtSecretKey { get; set; }
    public string? JwtIssuer { get; set; }
    public string? JwtAudience { get; set; }
}