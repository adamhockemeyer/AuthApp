using System;
using System.Threading.Tasks;

namespace AuthApp.Services
{
    public interface IAuthenticationService
    {
        string Username { get; }

        event Action<string> AuthenticationChanged;

        Task<string> GetToken(bool signInIfSilentFails = false);
        Task SignOut();
        Task<UserProfile> GetUserProfileAsync();
    }

    public class UserProfile
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }
    }

    public enum TokenAccessScope
    {
        GraphAPI,
        BackendAPI
    }
}
