namespace Tracker.Api.Controllers.Resources
{
    public class IssueResponseSaveResource
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int IssueTrackerId { get; set; }
    }
}