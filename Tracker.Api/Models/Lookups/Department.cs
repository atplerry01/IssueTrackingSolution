using Tracker.Api.Models.Common;

namespace Tracker.Api.Models.Lookups
{
    public class Department: BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortCode { get; set; }
    }
}