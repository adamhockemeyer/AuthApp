using System;
using System.Threading.Tasks;

namespace AuthApp.Services
{
    public class LoggerService : ILoggerService
    {
        public LoggerService()
        {
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task LogException(Exception exception)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            System.Diagnostics.Debug.WriteLine(exception.Message);
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task LogMessage(string message, string category)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            System.Diagnostics.Trace.WriteLine(message, category);
        }
    }
}
