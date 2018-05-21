using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Tracker.Api.Controllers.Resources
{
    public class IssueTrackerResource
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string DepartmentId { get; set; }
        public string IssueTypeId { get; set; }
        public KeyValuePairResource Department { get; set; }
        public KeyValuePairResource IssueType { get; set; }
        public DateTime DueDate { get; set; }

        public DateTime CreateOn { get; set; }
        public string CreatedBy { get; set; }

        public ICollection<IssueResponseResource> Responses { get; set; }

        public IssueTrackerResource()
        {
            Responses = new Collection<IssueResponseResource>();
        }
    }
}