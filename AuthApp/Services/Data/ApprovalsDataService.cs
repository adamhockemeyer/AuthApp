using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AuthApp.Common.Models;
using AuthApp.Constants;

namespace AuthApp.Services.Data
{
    public class ApprovalsDataService : BaseDataService
    {
        readonly IAuthenticationService _authService;
        readonly string[] _authScope;

        public ApprovalsDataService(IAuthenticationService authService)
        {
            _authService = authService;

            _authScope = Authentication.SCOPES_API;

            SetBaseAddress(DataServices.BASE_URL);
            SetHeaderValue("x-functions-key", DataServices.FUNCTION_AUTHENTICATION_KEY);
            ShowLoadingMessage(true);
        }

        public async Task<List<Approval>> GetApprovalsAsync()
        {
            SetBearerToken(await _authService.GetToken(_authScope, true));

            return await base.GetDataAsync<List<Approval>>("v1/Approvals");
        }
    }
}
