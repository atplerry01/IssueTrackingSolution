using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Tracker.Api.Providers;
using Tracker.Api.Models;
using Tracker.Api.Persistence;

namespace Tracker.Api.Utils
{
    public class GetLoginToken
    {
        public static TokenProviderOptions GetOptions()
        {
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.Config.GetSection("TokenAuthentication:SecretKey").Value));

            return new TokenProviderOptions
            {
                Path = Configuration.Config.GetSection("TokenAuthentication:TokenPath").Value,
                Audience = Configuration.Config.GetSection("TokenAuthentication:Audience").Value,
                Issuer = Configuration.Config.GetSection("TokenAuthentication:Issuer").Value,
                Expiration = TimeSpan.FromMinutes(Convert.ToInt32(Configuration.Config.GetSection("TokenAuthentication:ExpirationMinutes").Value)),
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
            };
        }

        public static LoginResponseData Execute(ApplicationUser user, Client client, ApplicationDbContext db, RefreshToken refreshToken=null)
        {
            var options = GetOptions();
            var now = DateTime.UtcNow;

            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(now).ToUniversalTime().ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            };

            var userClaims = db.UserClaims.Where(i => i.UserId == user.Id);
            foreach (var userClaim in userClaims)
            {
                claims.Add(new Claim(userClaim.ClaimType, userClaim.ClaimValue));
            }
            var userRoles = db.UserRoles.Where(i => i.UserId == user.Id);
            var serialRoles = Newtonsoft.Json.JsonConvert.SerializeObject(userRoles);

            foreach(var userRole in userRoles)
            {
                var role = db.Roles.Single(i => i.Id == userRole.RoleId);
                claims.Add(new Claim(Extensions.RoleClaimType, role.Name));
            }


             if (refreshToken == null)
            {
                refreshToken = new RefreshToken()
                {
                    UserId = user.Id,
                    Token = Guid.NewGuid().ToString("N"),
                };
                db.InsertNew(refreshToken);
            }

            refreshToken.IssuedUtc = now;
            refreshToken.ExpiresUtc = now.Add(options.Expiration);
            db.SaveChanges();

            
            var jwt = new JwtSecurityToken(
                issuer: options.Issuer,
                audience: options.Audience,
                claims: claims.ToArray(),
                notBefore: now,
                expires: now.Add(options.Expiration),
                signingCredentials: options.SigningCredentials);
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new LoginResponseData
            {
                access_token = encodedJwt,
                refresh_token = refreshToken.Token,
                expires_in = (int)options.Expiration.TotalSeconds,
                client_id = client.Id,
                userName = user.UserName,
                firstName = user.FirstName,
                lastName = user.LastName,
                roles = serialRoles,
                isAdmin = claims.Any(i => i.Type == Extensions.RoleClaimType && i.Value == Extensions.AdminRole)
            };
            return response;
        }
    }

}