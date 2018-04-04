using System;
namespace AuthApp.Constants
{
    public static class DataServices
    {
        //public const string BASE_URL = "http://localhost:7071/api/";
        public const string BASE_URL = "https://authapp-functions.azurewebsites.net/api/";
        // Used for calls to Azure Functions that have Functions Authentication enabled.
        public const string FUNCTION_AUTHENTICATION_KEY = "uLf56Rf9OZW9liJm7yNjLX9mWoOXawgWzma5wjp5PRvtCxKFCJj59Q==";
        public const string MS_GRAPH_URL = "https://graph.microsoft.com/";
    }
}
