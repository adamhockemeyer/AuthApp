
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

using AuthApp.Common.Models;


namespace AuthApp.API
{

    public static class Tasks
    {
        static Tasks()
        {
            
        }

        [FunctionName("GetTasks")]
        public static async Task<HttpResponseMessage> GetTasks
        (
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "v1/Tasks")]
            HttpRequestMessage req,
            TraceWriter log
        )
        {

            // Dummy Data - change out code for a call to your backend data store.
            Random r = new Random();
            int rDelay = r.Next(50, 800);
            // Simulate a delay for fetching real data.
            await Task.Delay(rDelay);

            List<TaskItem> _data = new List<TaskItem>
            {
                new TaskItem{
                    Id = Guid.NewGuid().ToString(),
                    Title = "Task 1",
                    Description = "Description 1",
                    Type = "Task",
                    CreatedOn = DateTime.Now, 
                    CompletedOn = null,
                    CreatorId = Guid.NewGuid().ToString(),
                     CreatorName = "Sue Smith"

                },
                new TaskItem{
                    Id = Guid.NewGuid().ToString(),
                    Title = "Task 2",
                    Description = "Description 2",
                    Type = "",
                    CreatedOn = DateTime.Now,
                    CompletedOn = null,
                    CreatorId = Guid.NewGuid().ToString(),
                     CreatorName = "Sue Smith"

                }
            };

            return req.CreateResponse<List<TaskItem>>(HttpStatusCode.BadRequest, _data);
        }
    }
}
