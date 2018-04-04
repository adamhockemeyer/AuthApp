using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.Identity.Client;
using Newtonsoft.Json.Linq;

using AuthApp.Constants;
using System.Collections.Generic;

namespace AuthApp.Services
{
    /// <summary>
    /// Authentication service implemented using MSAL and the Microsoft.Identity.Client library.
    /// https://github.com/AzureAD/microsoft-authentication-library-for-dotnet
    /// </summary>
    public class AuthenticationService : IAuthenticationService
    {
        ILoggerService _logger;
        ISimulatorCheck _simCheck;

        PublicClientApplication PCA;
        // UIParent is required by Android only and ties the Auth flow to the current activity.
        UIParent UiParent;

        // Cache for scopes and authentication result.
        Dictionary<string[], AuthenticationResult> authResultForScopes = new Dictionary<string[], AuthenticationResult>();

        // RedirectUri also needs set on the info.plist on iOS and the AndroidManifest.xml on Android.
        string RedirectUri => $@"msal{Authentication.CLIENT_ID}://auth";

        public string Name { get; set; }
        public string UserId { get; set; }

        public AuthenticationService() { }

        public AuthenticationService(ILoggerService logger, ISimulatorCheck simCheck)
        {
            _logger = logger;
            _simCheck = simCheck;


            if(Authentication.CLIENT_ID.Length != 36)
            {

                var exception = new AuthenticationServiceException("Please set the ClientID/Application ID in the Constants/Authentication.cs file.  " +
                                                         "If you have not registered your app, please visit https://apps.dev.microsoft.com, " +
                                                         "and register a 'Native Application'");

                _logger.LogException(exception);

                throw exception;        
            }

            PCA = new PublicClientApplication(Authentication.CLIENT_ID) { RedirectUri = RedirectUri};
        }

        /// <summary>
        /// Username of the current user.
        /// </summary>
        /// <value>The username.</value>
        //public string Username => authResult?.User?.Name ?? string.Empty;

        /// <summary>
        /// Action that fires when the users authentication has changed.
        /// </summary>
        /// <value>The authentication changed.</value>
        public event Action<string, string[]> AuthenticationChanged;

        /// <summary>
        /// Gets an access token for the current user.  Optionally if no token is available, we can prompt the user to sign in.
        /// </summary>
        /// <returns>The token.</returns>
        /// <param name="signInIfSilentFails">If set to <c>true</c> sign in if silent fails.</param>
        public async Task<string> GetToken(string[] Scopes, bool signInIfSilentFails = false)
        {
            try
            {
                //// Hack for MSAL Tokens not storing on the simulator
                //// On a real device, we want to check the cache every time for the access token.
                if (authResultForScopes.ContainsKey(Scopes) && _simCheck.CheckIfSimulator())
                {
                    return authResultForScopes[Scopes].AccessToken;
                }

                // Attempt to perform silent authentication (i.e. use previous authentication/refresh token.
                var authResult = await PCA.AcquireTokenSilentAsync(Scopes, PCA.Users.FirstOrDefault());

                authResultForScopes.Remove(Scopes);
                authResultForScopes.Add(Scopes, authResult);

                Name = authResult?.User?.Name;
                UserId = authResult?.User?.DisplayableId;

                await _logger.LogMessage($"Adding Scope(s): {String.Join(", ", Scopes)} to cache.", "Authentication");

                AuthenticationChanged?.Invoke(authResult.AccessToken, Scopes);

                return authResult.AccessToken;
            }
            catch (Exception ex)
            {
                await _logger.LogException(ex).ConfigureAwait(false);
                await _logger.LogMessage("Unable to get access token silently.", "Authentication").ConfigureAwait(false);

                if (signInIfSilentFails)
                {
                    try
                    {
                        // Attempt to perform an interactive login.
                        var authResult = await PCA.AcquireTokenAsync(Scopes, UiParent);

                        authResultForScopes.Remove(Scopes);
                        authResultForScopes.Add(Scopes, authResult);

                        Name = authResult?.User?.Name;
                        UserId = authResult?.User?.DisplayableId;

                        await _logger.LogMessage($"Adding Scope(s): {String.Join(", ", Scopes)} to cache.", "Authentication");

                        AuthenticationChanged?.Invoke(authResult.AccessToken, Scopes);

                        return authResult.AccessToken;
                    }
                    catch (Exception ex2)
                    {
                        await _logger.LogException(ex2).ConfigureAwait(false);
                        await _logger.LogMessage("Unable to perform authentication.", "Authentication").ConfigureAwait(false);
                    }
                }
            }

            // Return null if we weren't able to authenticate the user.
            return null;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        /// <summary>
        /// Force a sign out of the current user. NOTE: Currently MSAL only can remove a user from the Cache.
        /// </summary>
        /// <returns>The out.</returns>
        public async Task SignOut()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            foreach(IUser user in PCA.Users)
            {
                PCA.Remove(user);
            }

            Name = null;
            UserId = null;

            authResultForScopes.Clear();

            AuthenticationChanged?.Invoke(null, null);
        }

        public AuthenticationService SetUIParent(UIParent uiParent)
        {
            UiParent = uiParent;

            return this;
        }
    }

    /// <summary>
    /// Exception in the AuthenticationService.
    /// </summary>
    public class AuthenticationServiceException : Exception
    {
        public AuthenticationServiceException(string message) : base(message)
        {
            
        }

        public AuthenticationServiceException (string message, Exception inner) : base (message, inner)
        {
            
        }
    }
}
