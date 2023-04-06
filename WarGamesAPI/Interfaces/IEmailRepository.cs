using System.Threading.Tasks;
using WarGamesAPI.DTO;
namespace WarGamesAPI.Interfaces
{
    public interface IEmailRepository
    {
        Task SendEmailAsync(EmailRequestDto mailRequest);
        Task SendWelcomeEmailAsync(WelcomeRequestDto request);
        Task SendResetPasswordEmailAsync(string Url, string ToEmail, string UserName);
    }
}
