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
        private readonly string _issuer;
        private readonly string _audience;
        private const string Algorithm = SecurityAlgorithms.HmacSha256;

        public TokenHelper(IOptions<JWTSettings> jwtOptions)
        {
            _jwtSettings = jwtOptions.Value;
            _accessKey = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);
            _refreshKey = Encoding.UTF8.GetBytes(_jwtSettings.RefreshKey);
            _issuer = _jwtSettings.Issuer;
            _audience = _jwtSettings.Audience;
        }

        // Generates Access Token with Expiration
        public string GenerateAccessToken(Dictionary<string, string> payload)
        {
            var claims = payload.Select(pl => new Claim(pl.Key, pl.Value)).ToList();

            // Add Expiration Manually as a Claim
            var expiration = DateTime.UtcNow.AddSeconds(_jwtSettings.ExpiresIn);
            claims.Add(new Claim("expiration", ((DateTimeOffset)expiration).ToUnixTimeSeconds().ToString()));

            var credentials = new SigningCredentials(new SymmetricSecurityKey(_accessKey), Algorithm);

            var token = new JwtSecurityToken(
                //issuer: _jwtSettings.Issuer,
                //audience: _jwtSettings.Audience,
                claims: claims,
                expires: expiration,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Generates Refresh Token with different Key and Expiration
        public string GenerateRefreshToken(Dictionary<string, string> payload)
        {
            var claims = payload.Select(pl => new Claim(pl.Key, pl.Value)).ToList();

            var expiration = DateTime.UtcNow.AddSeconds(_jwtSettings.RefreshExpiresIn);
            claims.Add(new Claim("expiration", ((DateTimeOffset)expiration).ToUnixTimeSeconds().ToString()));

            var credentials = new SigningCredentials(new SymmetricSecurityKey(_refreshKey), Algorithm);

            var token = new JwtSecurityToken(
                claims: claims,
                //issuer: _jwtSettings.Issuer,  
                //audience: _jwtSettings.Audience,
                expires: expiration,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Verifies and Decodes Access Token
        public ClaimsPrincipal VerifyAccessToken(string token)
        {
            return ValidateToken(token, _accessKey, isRefresh: false);
        }

        // Verifies and Decodes Refresh Token
        public ClaimsPrincipal VerifyRefreshToken(string token)
        {
            return ValidateToken(token, _refreshKey, isRefresh: true);
        }

        // Validate Tokens
        public ClaimsPrincipal ValidateToken(string token, byte[] key, bool isRefresh)
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

        // Validate Token
        //public ClaimsPrincipal ValidateToken(string token, bool isRefresh = false)
        //{
        //    if (string.IsNullOrWhiteSpace(token))
        //        return null;

        //    var tokenHandler = new JwtSecurityTokenHandler();

        //    // Choose the Key depending on whether it is Access or Refresh Token
        //    var key = isRefresh ? _refreshKey : _accessKey;

        //    var parameters = new TokenValidationParameters
        //    {
        //        ValidateIssuer = false,
        //        ValidateAudience = false,
        //        ValidateLifetime = true, // Ensure Token is not Expired
        //        ValidateIssuerSigningKey = true,
        //        IssuerSigningKey = new SymmetricSecurityKey(key),
        //        ClockSkew = TimeSpan.Zero // No Tolerance for Expired Tokens
        //    };

        //    try
        //    {
        //        var principal = tokenHandler.ValidateToken(token, parameters, out var validatedToken);

        //        // (Optional) Additional Validation: Make sure it's a Valid JWT with Expected Algorithm
        //        if (validatedToken is JwtSecurityToken jwtToken)
        //        {
        //            var validAlgorithm = jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
        //            if (!validAlgorithm)
        //            {
        //                // Invalid Algorithm
        //                return null;
        //            }
        //        }

        //        return principal;

        //    }
        //    catch (SecurityTokenExpiredException)
        //    {
        //        // Token is Expired
        //        return null;
        //    }
        //    catch (Exception)
        //    {
        //        // Other Validation Issues
        //        return null;
        //    }
        //}

    }
}
