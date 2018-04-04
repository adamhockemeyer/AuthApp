using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

using AuthApp.Common.Models;
using System.Net;
using System.Security.Claims;

namespace AuthApp.API.Tasks
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

            var auth = await Helpers.Security.ValidateTokenAsync(req.Headers.Authorization);


            // Dummy Data - change out code for a call to your backend data store.

            // Simulate a delay for fetching real data.
            Random r = new Random();
            int rDelay = r.Next(50, 800);
            await Task.Delay(rDelay);

            List<TaskItem> _data = new List<TaskItem>
            {
                new TaskItem{
                    Id = Guid.NewGuid().ToString(),
                    Title = "2018 Budget Review",
                    Description = "Review 2018 budget proposal",
                    Type = "Budget",
                    CreatedOn = DateTime.Now,
                    CompletedOn = null,
                    CreatorId = Guid.NewGuid().ToString(),
                    CreatorName = "Sue Smith"

                },
                new TaskItem{
                    Id = Guid.NewGuid().ToString(),
                    Title = "PTO Approval",
                    Description = "Review/Approve PTO schedule",
                    Type = "Approval",
                    CreatedOn = DateTime.Now,
                    CompletedOn = null,
                    CreatorId = Guid.NewGuid().ToString(),
                    CreatorName = "John Williams"

                },
                new TaskItem{
                    Id = Guid.NewGuid().ToString(),
                    Title = "PTO Approval",
                    Description = "Review Jason's PTO",
                    Type = "Approval",
                    CreatedOn = DateTime.Now,
                    CompletedOn = null,
                    CreatorId = Guid.NewGuid().ToString(),
                    CreatorName = "Jason Shorts"

                },
                new TaskItem{
                    Id = Guid.NewGuid().ToString(),
                    Title = "Remove Legacy Server Rack",
                    Description = "Remove DB2 Server Rack",
                    Type = "Infrastructure",
                    CreatedOn = DateTime.Now,
                    CompletedOn = null,
                    CreatorId = Guid.NewGuid().ToString(),
                    CreatorName = "Jason Shorts"

                }
                ,
                new TaskItem{
                    Id = Guid.NewGuid().ToString(),
                    Title = "Replace Legacy Laptops",
                    Description = "Provision new laptops for users with laptops older than 4 years.",
                    Type = "Infrastructure",
                    CreatedOn = DateTime.Now,
                    CompletedOn = null,
                    CreatorId = Guid.NewGuid().ToString(),
                    CreatorName = "Laura Holdenly"

                }
            };

            return req.CreateResponse<List<TaskItem>>(HttpStatusCode.OK, _data);
        }
    }
}
