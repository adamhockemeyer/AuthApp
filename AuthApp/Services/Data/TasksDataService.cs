using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AuthApp.Common.Models;
using AuthApp.Constants;

namespace AuthApp.Services.Data
{
    public class TasksDataService : BaseDataService
    {
        IAuthenticationService _authService;

        public TasksDataService(IAuthenticationService authService)
        {
            _authService = authService;

            SetBaseAddress(DataServices.BASE_URL);
            ShowLoadingMessage(true);
        }

        public async Task<List<TaskItem>> GetTasksAsync()
        {
            SetBearerToken(await _authService.GetToken());

            return await base.GetDataAsync<List<TaskItem>>("v1/Tasks");
        }
    }
}
