using System.Threading.Tasks;

namespace CompetencyMatrix.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
