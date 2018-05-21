using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Tracker.Api.Models;
using Tracker.Api.Utils;

namespace Tracker.Api.Persistence
{
     public class ApplicationDbSeed
    {
        public ApplicationDbSeed(UserManager<ApplicationUser> userManager)
        {
        }

        public static async Task Seed(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            await SeedRolesAndClaims(userManager, roleManager);
            await SeedAdmin(userManager);
            await SeedClient(context);
        }


        private static async Task SeedRolesAndClaims(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {

            if (!await roleManager.RoleExistsAsync(Extensions.AdminRole))
            {
                await roleManager.CreateAsync(new IdentityRole
                {
                    Name = Extensions.AdminRole
                });
            }

            if (!await roleManager.RoleExistsAsync(Extensions.UserRole))
            {
                await roleManager.CreateAsync(new IdentityRole
                {
                    Name = Extensions.UserRole
                });
            }


            var adminRole = await roleManager.FindByNameAsync(Extensions.AdminRole);
            var adminRoleClaims = await roleManager.GetClaimsAsync(adminRole);

            if (!adminRoleClaims.Any(x => x.Type == Extensions.ManageUserClaim))
            {
                await roleManager.AddClaimAsync(adminRole, new System.Security.Claims.Claim(Extensions.ManageUserClaim, "true"));
            }
            if (!adminRoleClaims.Any(x => x.Type == Extensions.AdminClaim))
            {
                await roleManager.AddClaimAsync(adminRole, new System.Security.Claims.Claim(Extensions.AdminClaim, "true"));
            }

            var userRole = await roleManager.FindByNameAsync(Extensions.UserRole);
            var userRoleClaims = await roleManager.GetClaimsAsync(userRole);
            if (!userRoleClaims.Any(x => x.Type == Extensions.UserClaim))
            {
                await roleManager.AddClaimAsync(userRole, new System.Security.Claims.Claim(Extensions.UserClaim, "true"));
            }
        }

        private static async Task SeedAdmin(UserManager<ApplicationUser> userManager)
        {
            var u = await userManager.FindByNameAsync("admin");
            if (u == null)
            {
                u = new ApplicationUser
                {
                    UserName = "admin",
                    Email = "admin@nothing.com",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    IsEnabled = true,
                    FirstName = "admin",
                    LastName = "user"
                };
                var x = await userManager.CreateAsync(u, "Admin1234!");
            }
            var uc = await userManager.GetClaimsAsync(u);
            if (!uc.Any(x => x.Type == Extensions.AdminClaim))
            {
                await userManager.AddClaimAsync(u, new System.Security.Claims.Claim(Extensions.AdminClaim, true.ToString()));
            }
            if (!await userManager.IsInRoleAsync(u, Extensions.AdminRole))
                await userManager.AddToRoleAsync(u, Extensions.AdminRole);
        }

        private static async Task SeedClient(ApplicationDbContext context)
        {
            if (context.Clients.Count() > 0)
            {
                return;
            }

            context.Clients.AddRange(BuildClientsList());
            context.SaveChanges();
        }

        private static List<Client> BuildClientsList()
        {

            List<Client> ClientsList = new List<Client> 
            {
                new Client
                { Id = "ngAuthApp", 
                    Secret= Helper.GetHash("abc@123"), 
                    Name="AngularJS front-end Application", 
                    ApplicationType =  ApplicationTypes.JavaScript, 
                    Active = true, 
                    RefreshTokenLifeTime = 7200, 
                    AllowedOrigin = "http://ngauthenticationweb.azurewebsites.net"
                },
                new Client
                { Id = "consoleApp", 
                    Secret=Helper.GetHash("123@abc"), 
                    Name="Console Application", 
                    ApplicationType = ApplicationTypes.NativeConfidential, 
                    Active = true, 
                    RefreshTokenLifeTime = 14400, 
                    AllowedOrigin = "*"
                }
            };

            return ClientsList;
        }

    }
}