using Identity.BLL.Abstractions.Externals;
using Identity.DTO.Accounts;
using Identity.Entity.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Identity.BLL.ServiceImplementation
{
    public sealed class TokenService : ITokenService
    { 
        private readonly JwtSettings _jwtSetting;
        public TokenService(IOptions<JwtSettings> option)
        {
            _jwtSetting = option.Value;
        }

        public string CreateAccessToken(User user, string[] roles = null)
        {
            var now = DateTime.UtcNow;

            var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        new Claim(JwtRegisteredClaimNames.Email, user.Email),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(JwtRegisteredClaimNames.Iat,new DateTimeOffset(now).ToUnixTimeSeconds().ToString(),ClaimValueTypes.Integer64)};
            if (roles is not null)
                foreach (var role in roles)
                    claims.Add(new Claim(ClaimTypes.Role, role));
            var secret = Environment.GetEnvironmentVariable("JwtSettings__SecretKey")
                         ?? _jwtSetting.SecretKey; 
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = now.AddMinutes(_jwtSetting.ExpireAt);
            var token = new JwtSecurityToken(
                issuer: _jwtSetting.Issuer,
                audience: _jwtSetting.Audience,
                claims: claims,
                notBefore: now,
                expires: expires,   
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
