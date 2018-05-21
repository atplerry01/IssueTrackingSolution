using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tracker.Api.Controllers.Resources;
using Tracker.Api.Models.Lookups;
using Tracker.Api.Persistence;

namespace Tracker.Api.Controllers
{
    [Route("api/lookups")]
    public class LookupsController
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public LookupsController(ApplicationDbContext context, IMapper mapper)
        {
            this.mapper = mapper;
            this.context = context;
        }

        [HttpGet("departments")]
        public async Task<IEnumerable<DepartmentResource>> GetDepartments()
        {
            var departments = await context.Departments.ToListAsync();
            return mapper.Map<List<Department>, List<DepartmentResource>>(departments);
        }

        [HttpGet]
        public async Task<Object> Lookups()
        {
            var departments = await context.Departments.ToListAsync(); //GetDepartments();
            var issueTypes = await context.IssueTypes.ToListAsync();

            return new
            {
                departments,
                issueTypes
            };

        }

    }
}