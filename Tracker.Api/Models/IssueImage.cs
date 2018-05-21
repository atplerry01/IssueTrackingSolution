using System.ComponentModel.DataAnnotations;
using Tracker.Api.Models.Common;

namespace Tracker.Api.Models
{
    public class IssueImage: BaseEntity
    {
        [Required]
        [StringLength(255)]
        public string FileName { get; set; }
    }
}