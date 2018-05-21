using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tracker.Api.Models.Lookups;
using Tracker.Api.Persistence.Repository.Common;

namespace Tracker.Api.Controllers
{
    [Route("api/departments/")]
    public class DepartmentController : Controller
    {
        private readonly IGenericRepository<Department> departmentRepo;
        public DepartmentController(IGenericRepository<Department> departmentRepo)
        {
            this.departmentRepo = departmentRepo;

        }

        [HttpGet]
        public async Task<IEnumerable<Department>> GetIssueTrackers()
        {
            return departmentRepo.All();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetIssueTracker(int id)
        {
            var depart  = departmentRepo.FindById(id);
            return Ok(depart);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateIssueTracker([FromBody] Department department)
        {
            if (!ModelState.IsValid) return BadRequest();
            var depart = await departmentRepo.AddAsync(department);
            return Ok(depart);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateIssueTracker(int id, [FromBody] Department department)
        {
            if (!ModelState.IsValid) return BadRequest();
            var depart = await departmentRepo.UpdateAsync(id, department);           
            return Ok(depart);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteIssueTracker(int id)
        {
            await departmentRepo.DeleteAsync(id);
            return Ok(id);
        }

    }
}