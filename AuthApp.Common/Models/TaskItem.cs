using System;
namespace AuthApp.Common.Models
{
    public class TaskItem
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }

        public string CreatorName { get; set; }
        public string CreatorId { get; set; }

        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? CompletedOn { get; set; }



        public TaskItem()
        {
        }
    }
}
