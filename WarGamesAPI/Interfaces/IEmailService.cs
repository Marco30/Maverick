using WarGamesAPI.DTO;

namespace WarGamesAPI.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailRequestDto mailRequest);
        Task SendWelcomeEmailAsync(WelcomeRequestDto request);
        Task SendResetPasswordEmailAsync(string Url, string ToEmail, string UserName);

    }
}
