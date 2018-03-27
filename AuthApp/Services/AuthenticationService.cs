using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.Identity.Client;
using Newtonsoft.Json.Linq;

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
        /// <summary>
        /// UIParent is required by Android only and ties the Auth flow to the current activity.
        /// </summary>
        UIParent UiParent;
        AuthenticationResult authResult;
        string[] authResultScope;
        UserProfile userProfile;
        // Register your app at: https://apps.dev.microsoft.com
        // Then enter the ClientID/Application ID below.
        string ClientID = "bce81670-6944-407a-9cdb-97094bfa655a";
        // Access the Microsoft Graph
        string[] GraphScopes = { "User.Read" }; 
        // Access our Backend API
        string[] APIScopes = { "api://bce81670-6944-407a-9cdb-97094bfa655a/access_as_user" };



        /// <summary>
        /// RedirectUri also needs set on the info.plist on iOS and the AndroidManifest.xml on Android.
        /// </summary>
        /// <value>The redirect URI.</value>
        string RedirectUri => $@"msal{ClientID}://auth";

        HttpClient _httpClient;

        public AuthenticationService() { }

        public AuthenticationService(ILoggerService logger, ISimulatorCheck simCheck)
        {
            _logger = logger;
            _simCheck = simCheck;


            if(ClientID.Length != 36)
            {

                var exception = new AuthenticationServiceException("Please set the ClientID/Application ID in the AuthenticationService.cs file.  " +
                                                         "If you have not registered your app, please visit https://apps.dev.microsoft.com, " +
                                                         "and register a 'Native Application'");

                _logger.LogException(exception);

                throw exception;        
            }

            PCA = new PublicClientApplication(ClientID) { RedirectUri = RedirectUri};
        }

        /// <summary>
        /// Username of the current user.
        /// </summary>
        /// <value>The username.</value>
        public string Username => authResult?.User?.Name ?? string.Empty;

        /// <summary>
        /// Action that fires when the users authentication has changed.
        /// </summary>
        /// <value>The authentication changed.</value>
        public event Action<string> AuthenticationChanged;

        /// <summary>
        /// Gets an access token for the current user.  Optionally if no token is available, we can prompt the user to sign in.
        /// </summary>
        /// <returns>The token.</returns>
        /// <param name="signInIfSilentFails">If set to <c>true</c> sign in if silent fails.</param>
        public async Task<string> GetToken(bool signInIfSilentFails = false)
        {
            try
            {
                //// Hack for MSAL Tokens not storing on the simulator
                //// On a real device, we want to check the cache every time for the access token.
                if (authResult != null && _simCheck.CheckIfSimulator())
                {
                    return authResult.AccessToken;
                }

                // Attempt to perform silent authentication (i.e. use previous authentication/refresh token.
                authResult = await PCA.AcquireTokenSilentAsync(GraphScopes, PCA.Users.FirstOrDefault());
                //authResultScope = scope;

                AuthenticationChanged?.Invoke(authResult.AccessToken);

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
                        authResult = await PCA.AcquireTokenAsync(GraphScopes, UiParent);
                        //authResultScope = scope;

                        AuthenticationChanged?.Invoke(authResult.AccessToken);

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

            authResult = null;

            userProfile = null;

            AuthenticationChanged?.Invoke(null);
        }

        public async Task<UserProfile> GetUserProfileAsync()
        {
            UserProfile profile = userProfile;

            if(profile != null)
            {
                return profile;
            }

            _httpClient = _httpClient ?? new HttpClient();

            try
            {
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, "https://graph.microsoft.com/v1.0/me");
                message.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", await GetToken());
                HttpResponseMessage response = await _httpClient.SendAsync(message);
                string responseString = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    JObject user = JObject.Parse(responseString);

                    profile = new UserProfile
                    {
                        Id = user["id"].ToString(),
                        Name = user["displayName"].ToString(),
                        EmailAddress = user["mail"].ToString()
                    };
                }
                else
                {
                    await _logger.LogException(new Exception(response.ReasonPhrase)).ConfigureAwait(false);
                    await _logger.LogMessage("Unable to get user profile.", "Authentication").ConfigureAwait(false);

                    return null;
                }
            }
            catch (Exception ex)
            {
                await _logger.LogException(ex).ConfigureAwait(false);
                await _logger.LogMessage("Unable to get user profile.", "Authentication").ConfigureAwait(false);
            }



            return profile;
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
