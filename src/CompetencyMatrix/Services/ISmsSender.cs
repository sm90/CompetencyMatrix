using System.Threading.Tasks;

namespace CompetencyMatrix.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
