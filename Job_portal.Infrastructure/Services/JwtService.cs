using Job_portal.Application.Common.Interfaces;
using Job_portal.Application.Common.Settings;
using Job_portal.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Job_portal.Infrastructure.Services
{
    public class JwtService : IJwtService,IJwtServiceAdmin
    {
        private readonly JwtSettings _jwtSettings;

        public JwtService(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        // Called after Student/Recruiter login
        //frontend reads role → ProtectedRoute 
        //frontend reads userId → API calls
        public string GenerateToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub , user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email , user.Email),
                new Claim(ClaimTypes.Role , user.Role.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti , Guid.NewGuid().ToString())

            };
            return BuildToken(claims);

        }

        // Called after Admin login
        // ProtectedAdminRoute checks role === "Admin"
        public string GenerateAdminToken(string email)
        {
            var claims = new[]
            {
               new Claim (JwtRegisteredClaimNames.Email , email),
               new Claim(ClaimTypes .Role , "Admin"),
               new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
           };
            return BuildToken(claims);
        }

        private string BuildToken(Claim[] claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8
                                .GetBytes(_jwtSettings.Secret));//! => says it can not be null

            var credentials = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims:claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
