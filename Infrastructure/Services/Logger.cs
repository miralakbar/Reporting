using Serilog;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class Logger : Application.Interfaces.ILogger
    {
        public async Task LogConsoleAsync(string message)
        {
            Log.Information(message);
        }
    }
}