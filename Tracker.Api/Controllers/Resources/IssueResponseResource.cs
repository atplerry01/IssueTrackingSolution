using System;

namespace Tracker.Api.Controllers.Resources
{
    public class IssueResponseResource
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}