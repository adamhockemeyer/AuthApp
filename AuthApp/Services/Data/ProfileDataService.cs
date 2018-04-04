using System;
using System.Threading.Tasks;
using AuthApp.Constants;
using Newtonsoft.Json.Linq;

namespace AuthApp.Services.Data
{
    public class ProfileDataService : BaseDataService
    {
        readonly IAuthenticationService _authService;
        readonly string[] _authScope;

        public class UserProfile
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string EmailAddress { get; set; }
        }

        public ProfileDataService(IAuthenticationService authService)
        {
            _authService = authService;

            _authScope = Authentication.SCOPES_MS_GRAPH;

            SetBaseAddress(DataServices.MS_GRAPH_URL);
            ShowLoadingMessage(true);
        }

        public async Task<UserProfile> GetUserProfileAsync()
        {
            UserProfile profile = null;

            string token = await _authService.GetToken(_authScope);

            SetBearerToken(token);


            var response = await base.GetDataAsStringAsync("v1.0/me");


            if (string.IsNullOrEmpty(response))
            {
                return null;
            }

            JObject user = JObject.Parse(response);

            profile = new UserProfile
            {
                Id = user["id"].ToString(),
                Name = user["displayName"].ToString(),
                EmailAddress = user["mail"].ToString()
            };


            return profile;
        }
    }
}
