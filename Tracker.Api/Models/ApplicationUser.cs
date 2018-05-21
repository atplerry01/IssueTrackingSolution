using Microsoft.AspNetCore.Identity;
using Tracker.Api.Models;

namespace Tracker.Api.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsEnabled { get; set; }
    }
}