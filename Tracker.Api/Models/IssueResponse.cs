using System;
using Tracker.Api.Models.Common;

namespace Tracker.Api.Models
{
    public class IssueResponse: BaseEntity
    {
        public string Description { get; set; }
        public int IssueTrackerId { get; set; }
        public IssueTracker IssueTracker { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}