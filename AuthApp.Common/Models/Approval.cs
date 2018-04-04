using System;
using Newtonsoft.Json;

namespace AuthApp.Common.Models
{
    
    public class Approval
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("estimate_amount")]
        public decimal EstimateAmount { get; set; }

        [JsonProperty("createdby")]
        public string Createdby { get; set; }

        [JsonProperty("createdon")]
        public System.DateTimeOffset Createdon { get; set; }

        [JsonProperty("approval_history")]
        public ApprovalHistory[] ApprovalHistory { get; set; }
    }

    public class ApprovalHistory
    {
        [JsonProperty("step")]
        public long Step { get; set; }

        [JsonProperty("action")]
        public bool Action { get; set; }

        [JsonProperty("timestamp")]
        public System.DateTimeOffset Timestamp { get; set; }

        [JsonProperty("comments")]
        public string Comments { get; set; }

        [JsonProperty("approver_name")]
        public string ApproverName { get; set; }

        [JsonProperty("approver_email")]
        public string ApproverEmail { get; set; }
    }
}
