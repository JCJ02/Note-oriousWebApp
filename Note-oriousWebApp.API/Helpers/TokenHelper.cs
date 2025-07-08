using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Note_oriousWebApp.API.Configurations;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Note_oriousWebApp.API.Helpers
{
    public class TokenHelper
    {
        private readonly JWTSettings _jwtSettings;
        private readonly byte[] _accessKey;
        private readonly byte[] _refreshKey;
        private const string Algorithm = SecurityAlgorithms.HmacSha256;

        public TokenHelper(IOptions<JWTSettings> jwtOptions)
        {
            _jwtSettings = jwtOptions.Value;
            _accessKey = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);
            _refreshKey = Encoding.UTF8.GetBytes(_jwtSettings.RefreshKey); // Add this to your settings
        }

        // Generates access token with expiration
        public string GenerateAccessToken(Dictionary<string, string> payload)
        {
            var claims = payload.Select(pl => new Claim(pl.Key, pl.Value)).ToList();

            // Add expiration manually as a claim
            var expiration = DateTime.UtcNow.AddSeconds(_jwtSettings.ExpiresIn);
            claims.Add(new Claim("expiration", ((DateTimeOffset)expiration).ToUnixTimeSeconds().ToString()));

            var credentials = new SigningCredentials(new SymmetricSecurityKey(_accessKey), Algorithm);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: expiration,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Generates refresh token with different key and expiration
        public string GenerateRefreshToken(Dictionary<string, string> payload)
        {
            var claims = payload.Select(pl => new Claim(pl.Key, pl.Value)).ToList();

            var expiration = DateTime.UtcNow.AddSeconds(_jwtSettings.RefreshExpiresIn);
            claims.Add(new Claim("expiration", ((DateTimeOffset)expiration).ToUnixTimeSeconds().ToString()));

            var credentials = new SigningCredentials(new SymmetricSecurityKey(_refreshKey), Algorithm);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: expiration,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Verifies and decodes access token
        public ClaimsPrincipal VerifyAccessToken(string token)
        {
            return ValidateToken(token, _accessKey, isRefresh: false);
        }

        // Verifies and decodes refresh token
        public ClaimsPrincipal VerifyRefreshToken(string token)
        {
            return ValidateToken(token, _refreshKey, isRefresh: true);
        }

        private ClaimsPrincipal ValidateToken(string token, byte[] key, bool isRefresh)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var parameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                };

                return tokenHandler.ValidateToken(token, parameters, out _);
            }
            catch (Exception ex)
            {
                throw new SecurityTokenException($"{(isRefresh ? "Refresh" : "Access")} Token Validation Failed: {ex.Message}");
            }
        }
    }
}
