using System;
using System.Threading.Tasks;

namespace AuthApp.Services
{
    public interface IAuthenticationService
    {
        string Name { get; set; }
        string UserId { get; set; }
        event Action<string, string[]> AuthenticationChanged;

        Task<string> GetToken(string[] Scopes, bool signInIfSilentFails = false);
        Task SignOut();
    }



    public enum TokenAccessScope
    {
        GraphAPI,
        BackendAPI
    }
}
