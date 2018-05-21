using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using Tracker.Api.Models.Lookups;
using Tracker.Api.Models;
using Tracker.Api.Models.Common;

namespace Tracker.Api.Models
{
    public class IssueTracker: BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int DepartmentId { get; set; }
        public int IssueTypeId { get; set; }

        public DateTime DueDate { get; set; }

        public DateTime CreateOn { get; set; }
        public string CreatedBy { get; set; }

        [ForeignKey("DepartmentId")]
        public Department Department { get; set; }

        [ForeignKey("IssueTypeId")]
        public IssueType IssueType { get; set; }

        public ICollection<IssueResponse> Responses { get; set; }
        public ICollection<IssueImage> IssueImages { get; set; }

        public IssueTracker()
        {
            Responses = new Collection<IssueResponse>();
            IssueImages = new Collection<IssueImage>();
        }
    }
}