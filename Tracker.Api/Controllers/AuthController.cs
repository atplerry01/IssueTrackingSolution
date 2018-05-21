using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tracker.Api.Models;
using Tracker.Api.Persistence;
using Tracker.Api.Utils;

namespace Tracker.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Authentication")]
    [AllowAnonymous]
    public class AuthController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _db;

        public AuthController(SignInManager<ApplicationUser> signInManager,
                UserManager<ApplicationUser> userManager,
                ApplicationDbContext db)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _db = db;
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Authenticate(string username, string password, string client_id)
        {
            string clientId = string.Empty;
            Client client = null;
            clientId = client_id;

            if (clientId == null || clientId == "")
            {
                // context.Response.StatusCode = 400;
                // await context.Response.WriteAsync("invalid_clientId, ClientId should be sent.");
                // return;
            }

            //Find Client
            client = _db.Clients.Find(clientId);

            if (client == null)
            {
                return BadRequest("invalid_clientId, Client is not registered in the system");
            }

            if (!client.Active)
            {
                return BadRequest("invalid_clientId, Client is inactive.");
            }

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return BadRequest("username and password may not be empty");

            var result = await _signInManager.PasswordSignInAsync(username, password, false, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                return BadRequest("Invalid username or password.");
            }
            var user = await _userManager.Users
                .SingleAsync(i => i.UserName == username);
            if (!user.IsEnabled)
            {
                return BadRequest("Invalid username or password.");
            }
            var response = GetLoginToken.Execute(user, client, _db);
            return Ok(response);
        }

    }
}