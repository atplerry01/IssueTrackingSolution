using System;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tracker.Api.Controllers.Resources;
using Tracker.Api.Models;
using Tracker.Api.Persistence;

namespace Tracker.Api.Controllers
{
     [Route("api/issuetrackers/{issueTrackerId}/images")]
    public class IssueImagesController : Controller
    {

        private readonly IHostingEnvironment host;
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public IssueImagesController(ApplicationDbContext context, IMapper mapper, IHostingEnvironment host)
        {
            this.mapper = mapper;
            this.context = context;
            this.host = host;
        }
        [HttpPost]
        public async Task<IActionResult> Upload(int issueTrackerId, IFormFile file) //IFormFile file, IFormCollection files
        {

            var issueTracker = await context.IssueTrackers
                .Include(i => i.IssueType)
                .Include(d => d.Department)
                .Include(r => r.Responses)
                .Include(f => f.IssueImages)
                .SingleOrDefaultAsync(it => it.Id == issueTrackerId);

            if (issueTracker == null)
                return NotFound();

            var uploadFolderPath = Path.Combine(host.WebRootPath, "uploads");
            if (Directory.Exists(uploadFolderPath))
                Directory.CreateDirectory(uploadFolderPath);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadFolderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            //Todo: Creating a thumnail for the file
            var photo = new IssueImage { FileName = fileName };
            issueTracker.IssueImages.Add(photo);
            await context.SaveChangesAsync();

            return Ok(mapper.Map<IssueImage, IssueImageResource>(photo));
        }

    }
}