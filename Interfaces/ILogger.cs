using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ILogger
    {
        Task LogConsoleAsync(string message);
    }
}
