using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Tracker.Api.Controllers.Resources
{
    public class IssueTrackerSaveResource
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int DepartmentId { get; set; }
        public int IssueTypeId { get; set; }
        // public DateTime DueDate { get; set; }

        // public DateTime CreateOn { get; set; }
        public string CreatedBy { get; set; }

    }
}