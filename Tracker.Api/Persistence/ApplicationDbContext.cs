using System.Linq;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tracker.Api.Models;
using Tracker.Api.Models.Lookups;
using Tracker.Api.Persistence.Configuration;

namespace Tracker.Api.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
          : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.RemovePluralizingTableNameConvention();
            
            modelBuilder.Entity<RefreshToken>()
                .HasAlternateKey(c => c.UserId)
                .HasName("refreshToken_UserId");
            modelBuilder.Entity<RefreshToken>()
                .HasAlternateKey(c => c.Token)
                .HasName("refreshToken_Token");

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Department> Departments { get; set; }
        public DbSet<IssueType> IssueTypes { get; set; }
        public DbSet<IssueResponse> IssueResponses { get; set; }
        public DbSet<IssueTracker> IssueTrackers { get; set; }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Audience> Audiences { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }


        public void InsertNew(RefreshToken token)
        {
            var tokenModel = RefreshTokens.SingleOrDefault(i => i.UserId == token.UserId);
            if (tokenModel != null)
            {
                RefreshTokens.Remove(tokenModel);
                SaveChanges();
            }
            RefreshTokens.Add(token);
            SaveChanges();
        }

    }

}