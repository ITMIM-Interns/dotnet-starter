using Identity.BLL.Abstractions.Externals;
using Identity.DTO.Accounts;
using Identity.Entity.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Identity.BLL.ServiceImplementation
{
    public sealed class TokenService : ITokenService
    { 
        private readonly JwtSetting _jwtSetting;

        public TokenService(IOptions<JwtSetting> jwtSetting)
        {
            _jwtSetting = jwtSetting.Value;
        }

        public string CreateAccessToken(User user, string[] roles=null)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Nbf,now.AddSeconds(2).ToUnixTimeSeconds().ToString(),ClaimValueTypes.Integer64),
                new Claim(JwtRegisteredClaimNames.Iat,now.ToUnixTimeSeconds().ToString(),ClaimValueTypes.Integer64)
            };
            var env = Environment.GetEnvironmentVariable("JwtSettings__SecretKey")??throw new Exception("Secret key cannot be empty");
            if (roles is not null)
            {
                foreach(var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(env));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken
            (
                issuer:_jwtSetting.Issuer,
                audience:_jwtSetting.Audience,
                claims: claims,
                expires:DateTime.UtcNow.AddMinutes(_jwtSetting.ExpireAt),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token); 
        }
    }
}
