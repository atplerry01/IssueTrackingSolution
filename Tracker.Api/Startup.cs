using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GraphQL;
using GraphQL.Http;
using GraphQL.Types;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Tracker.Api.Graphs.InputTypes;
using Tracker.Api.Graphs.ObjectTypes;
using Tracker.Api.Graphs.Schemas;
using Tracker.Api.Models;
using Tracker.Api.Persistence;
using Tracker.Api.Persistence.Repository.Common;

namespace Tracker.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddAutoMapper();
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Default")));

            #region IentityRegion
            // add identity
            var builder = services.AddIdentityCore<ApplicationUser>(o =>
            {
                // configure identity options
                o.Password.RequireDigit = false;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 6;
            });
            builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), builder.Services);
            builder.AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("UserManagement", policy => policy.RequireClaim(Utils.Extensions.ManageUserClaim));
                options.AddPolicy("Admin", policy => policy.RequireClaim(Utils.Extensions.AdminClaim));
                options.AddPolicy("User", policy => policy.RequireClaim(Utils.Extensions.UserClaim));
                options.AddPolicy("RequireAdministratorRole", policy => policy.RequireRole(Utils.Extensions.AdminRole));
            });

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            // return 401 instead of redirect to login
            services.ConfigureApplicationCookie(options =>
            {
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.Headers["Location"] = context.RedirectUri;
                    context.Response.StatusCode = 401;
                    return Task.CompletedTask;
                };
            });
            #endregion

            #region AuthenticationRegion

            services.AddAuthentication(sharedOptions =>
        {
            sharedOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            sharedOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
                  .AddJwtBearer(cfg =>
                  {
                      cfg.RequireHttpsMetadata = false;
                      //cfg.SaveToken = true;

                      cfg.TokenValidationParameters = new TokenValidationParameters()
                      {
                          ValidateIssuer = true,
                          ValidateAudience = true,
                          ValidateLifetime = true,
                          ValidateIssuerSigningKey = true,
                          ValidIssuer = Configuration["TokenAuthentication:Issuer"],
                          ValidAudience = Configuration["TokenAuthentication:Audience"],
                          IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["TokenAuthentication:SecretKey"]))
                      };

                      cfg.Events = new JwtBearerEvents
                      {
                          OnAuthenticationFailed = context =>
                          {
                              Console.WriteLine("OnAuthenticationFailed: " +
                                  context.Exception.Message);
                              return Task.CompletedTask;
                          },
                          OnTokenValidated = context =>
                          {
                              Console.WriteLine("OnTokenValidated: " +
                                  context.SecurityToken);
                              return Task.CompletedTask;
                          }
                      };

                  });
            #endregion

            services.AddScoped<DbContext, ApplicationDbContext>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddSingleton<IDocumentExecuter, DocumentExecuter>();
            services.AddSingleton<IDocumentWriter, DocumentWriter>();

            services.AddSingleton<DepartmentType>();

            services.AddSingleton<DepartmentInputType>();

            services.AddScoped<TrackerSolutionQuery>();
            services.AddScoped<TrackerSolutionMutation>();

            var sp = services.BuildServiceProvider();
            services.AddScoped<ISchema>(_ => new TrackerSolutionSchema(type => (GraphType)sp.GetService(type)) { Query = sp.GetService<TrackerSolutionQuery>() });

            services.AddMvc();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseGraphiQl();
            app.UseMvc();
        }
    }
}
