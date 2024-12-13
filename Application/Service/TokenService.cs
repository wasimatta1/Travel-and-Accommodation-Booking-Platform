using Contracts.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Service
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        public TokenService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<string> GenerateTokenAsync(string Email, string Role)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, Email),
                new Claim(ClaimTypes.Role, Role),

            };
            SymmetricSecurityKey _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);

            if (!double.TryParse(_config["JWT:ExpirationMinutes"], out double Expires))
            {
                Expires = 30;
            }
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(Expires),
                SigningCredentials = creds,
                Issuer = _config["JWT: Issuer"],
                Audience = _config["JWT: Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public async Task<string> GenerateRefreshTokenAsync()
        {
            SymmetricSecurityKey _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:RefreshKey"]));
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);

            if (!double.TryParse(_config["JWT:RefreshExpirationMinutes"], out double Expires))
            {
                Expires = 30;
            }
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.Now.AddMinutes(Expires),
                SigningCredentials = creds,
                Issuer = _config["JWT: Issuer"],
                Audience = _config["JWT: Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public async Task<bool> ValidateRefreshToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config["Jwt:RefreshKey"]);

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                }, out SecurityToken validatedToken);

                return true;

            }
            catch
            {

                return false;
            }
        }

    }
}
