using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Tracker.Api.Controllers.Resources;
using Tracker.Api.Models;
using Tracker.Api.Persistence;

namespace Tracker.Api.Controllers
{
    [Route("api/issuetrackers/")]
    public class IssueTrackersController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly ILogger logger;
        public IssueTrackersController(ApplicationDbContext context, IMapper mapper, ILogger<IssueTrackersController> logger)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<IssueTrackerResource>> GetIssueTrackers()
        {
            var issueTrackers = await context.IssueTrackers
                .Include(i => i.IssueType)
                .Include(d => d.Department)
                .ToListAsync();
            return mapper.Map<IEnumerable<IssueTracker>, IEnumerable<IssueTrackerResource>>(issueTrackers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetIssueTracker(int id)
        {

            logger.LogInformation("GetIssueTracker", id);

            var issueTracker = await context.IssueTrackers
                .Include(i => i.IssueType)
                .Include(d => d.Department)
                .Include(r => r.Responses)
                .SingleOrDefaultAsync(it => it.Id == id);

            if (issueTracker == null) return NotFound();

            var issueTrackerResource = mapper.Map<IssueTracker, IssueTrackerResource>(issueTracker);

            return Ok(issueTrackerResource);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateIssueTracker([FromBody] IssueTrackerSaveResource issueTrackerSaveResource)
        {
            if (!ModelState.IsValid) return BadRequest();

            var issueTracker = mapper.Map<IssueTrackerSaveResource, IssueTracker>(issueTrackerSaveResource);
            issueTracker.CreateOn = DateTime.Now;
            issueTracker.DueDate = DateTime.Now.AddDays(2);

            context.IssueTrackers.Add(issueTracker);
            await context.SaveChangesAsync();

            issueTracker = await context.IssueTrackers
                .Include(i => i.IssueType)
                .Include(d => d.Department)
                .SingleOrDefaultAsync(it => it.Id == issueTracker.Id);

            var result = mapper.Map<IssueTracker, IssueTrackerResource>(issueTracker);
            return Ok(result);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateIssueTracker(int id, [FromBody] IssueTrackerSaveResource issueTrackerSaveResource)
        {
            if (!ModelState.IsValid) return BadRequest();

            var issueTracker = await context.IssueTrackers
                .Include(i => i.IssueType)
                .Include(d => d.Department)
                .SingleOrDefaultAsync(it => it.Id == id);

            if (issueTracker == null) return NotFound();

            mapper.Map<IssueTrackerSaveResource, IssueTracker>(issueTrackerSaveResource, issueTracker);

            await context.SaveChangesAsync();

            var issueTracker2 = await context.IssueTrackers
                    .Include(i => i.IssueType)
                    .Include(d => d.Department)
                    .SingleOrDefaultAsync(it => it.Id == id);

            var result = mapper.Map<IssueTracker, IssueTrackerResource>(issueTracker2);

            return Ok(result);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteIssueTracker(int id)
        {
            var issueTracker = await context.IssueTrackers.FindAsync(id);
            if (issueTracker == null) return NotFound();

            context.IssueTrackers.Remove(issueTracker);
            await context.SaveChangesAsync();

            return Ok(id);
        }

    }
}