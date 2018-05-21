using AutoMapper;
using Tracker.Api.Controllers.Resources;
using Tracker.Api.Models;
using Tracker.Api.Models.Lookups;

namespace Tracker.Api.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Domain to Api Resources
            CreateMap<Department, KeyValuePairResource>();
            CreateMap<IssueType, KeyValuePairResource>();
            CreateMap<IssueResponse, IssueResponseResource>();
            CreateMap<IssueTracker, IssueTrackerResource>();
            CreateMap<IssueImage, IssueImageResource>();

            // Api Resources to Domain
            CreateMap<IssueResponseSaveResource, IssueResponse>();
            CreateMap<IssueTrackerSaveResource, IssueTracker>()
                .ForMember(i => i.Id, opt => opt.Ignore());

        }
    }
}