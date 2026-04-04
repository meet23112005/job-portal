using Job_portal.Application.Common.Interfaces;
using Job_portal.Application.Common.Interfaces.Repositories;
using Job_portal.Application.Common.Settings;
using Job_portal.Infrastructure.Persistence;
using Job_portal.Infrastructure.Persistence.Repositories;
using Job_portal.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Job_portal.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureDI(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            //Option Pattern
            services.Configure<JwtSettings>(
                configuration.GetSection(JwtSettings.SectionName));
            services.Configure<AdminSettings>(
                configuration.GetSection(AdminSettings.SectionName));
            services.Configure<CloudinarySettings>(
                configuration.GetSection(CloudinarySettings.SectionName));


            //services.AddScoped<IFileService, CloudinaryFileService>();//Cloudinary
             services.AddScoped<IFileService, LocalFileService>(); //Local

            //Jwt service
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IJwtServiceAdmin, JwtService>();

            //repository
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IJobRepository, JobRepository>();
            services.AddScoped<IJobApplicationRepository, JobApplicationRepository>();
            services.AddScoped<ISavedJobRepository, SavedJobRepository>();

            //JWT Authantication
            var jwtSettings = configuration
                                .GetSection(JwtSettings.SectionName)
                                .Get<JwtSettings>()!;

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidateAudience = true,
                        ValidAudience = jwtSettings.Audience,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
                        ValidateLifetime = true
                    };

                    // ── Read token from HttpOnly Cookie ───────────
                    // frontend sends withCredentials: true
                    // token travels in cookie not Authorization header
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var token = context.Request.Cookies["token"];
                            if (string.IsNullOrEmpty(token))
                            {
                                var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
                                if (authHeader?.StartsWith("Bearer ") == true)
                                {
                                    token = authHeader.Substring("Bearer ".Length).Trim();
                                }
                            }
                            if (!string.IsNullOrEmpty(token))
                                context.Token = token;
                            return Task.CompletedTask;
                        }
                    };
                });

            //role-based authorization
            services.AddAuthorization(options =>
            {
                options.AddPolicy("StudentOnly", policy =>
                    policy.RequireRole("Student"));

                options.AddPolicy("RecruiterOnly", policy =>
                    policy.RequireRole("Recruiter"));

                options.AddPolicy("AdminOnly", policy =>
                    policy.RequireRole("Admin"));

                options.AddPolicy("StudentOrRecruiter", policy =>
                    policy.RequireRole("Student", "Recruiter"));
            });
            return services;
        }
    }
}
