using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

using AuthApp.Common.Models;
using System.Collections.Generic;

namespace AuthApp.API.Approvals
{
    public static class Approvals
    {
        static Approvals()
        {
        }

        [FunctionName("GetApprovals")]
        public static async Task<HttpResponseMessage> GetApprovals
        (
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "v1/Approvals")]
            HttpRequestMessage req,
            TraceWriter log
        )
        {
            var data = Helpers.ReadJsonFile.ReadJsonFileAs<List<Approval>>("Approvals.Approvals.json");

            return req.CreateResponse(System.Net.HttpStatusCode.OK, data);
        }
    }
}
