using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tracker.Api.Controllers.Resources;
using Tracker.Api.Models;
using Tracker.Api.Persistence;

namespace Tracker.Api.Controllers
{
    [Route("api/issue/{issueId}/responses")]
    public class IssueResponsesController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        public IssueResponsesController(IMapper mapper, ApplicationDbContext context)
        {
            this.mapper = mapper;
            this.context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<IssueResponseResource>> GetIssueResponses(int issueId)
        {
            var issueTrackers = await context.IssueResponses
                .Where(i => i.IssueTrackerId == issueId)
                .ToListAsync();
            return mapper.Map<IEnumerable<IssueResponse>, IEnumerable<IssueResponseResource>>(issueTrackers);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateIssueResponse(int issueId, [FromBody] IssueResponseSaveResource issueResponseSaveResource)
        {
            var issueTracker = await context.IssueTrackers
                .Include(i => i.IssueType)
                .Include(d => d.Department)
                .SingleOrDefaultAsync(it => it.Id == issueId);

            var responses = mapper.Map<IssueResponseSaveResource, IssueResponse>(issueResponseSaveResource);
            responses.CreatedOn = DateTime.Now;

            issueTracker.Responses.Add(responses);
            await context.SaveChangesAsync();

            //Todo: Add other navigation informations
            var result = mapper.Map<IssueResponse, IssueResponseResource>(responses);

            return Ok(result);
        }


    }
}