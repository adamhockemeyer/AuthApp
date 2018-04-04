using System;

namespace AuthApp.Constants
{
    public class Authentication
    {
        // Appication Id from https://apps.dev.microsoft.com for Authentication
        public static string CLIENT_ID = "bce81670-6944-407a-9cdb-97094bfa655a";
        // Scopes to read basic user profile from the Microsoft Graph API.
        public static string[] SCOPES_MS_GRAPH = { "User.Read" };
        // Our Web API App ID URI from https://apps.dev.microsoft.com
        public static string[] SCOPES_API = { "api://bce81670-6944-407a-9cdb-97094bfa655a/access_as_user" };
    }
}
