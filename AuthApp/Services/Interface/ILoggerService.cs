using System;

using System.Threading.Tasks;

namespace AuthApp.Services
{
    public interface ILoggerService
    {
        Task LogMessage(string message, string category);
        Task LogException(Exception exception);
    }
}
