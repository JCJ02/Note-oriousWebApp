using Microsoft.AspNetCore.Mvc;
using Note_oriousWebApp.API.DTOs.UsersDTOs;
using Note_oriousWebApp.API.Helpers;
using Note_oriousWebApp.API.Services;
using System.Security.Claims;

namespace Note_oriousWebApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // This Sets the Base Endpoint to: api/Auth
    public class AuthController : ControllerBase
    {
        // Call the UsersService Class
        private readonly AuthService _authService;
        private readonly TokenHelper _tokenHelper;

        // Constructor
        public AuthController(AuthService authService, TokenHelper tokenHelper)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _tokenHelper = tokenHelper;
        }

        // LOGIN a User Method
        // POST /api/Auth/
        [HttpPost]
        public async Task<IActionResult> Auth([FromBody] UserAuthDTO userAuthDTO)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userAuthDTO.Email))
                {
                    return BadRequest(new { message = "Email is Required!" });
                }
                else if (!ValidationHelper.IsValidEmail(userAuthDTO.Email))
                {
                    return BadRequest(new { message = "Invalid Email Address!" });
                }
                else if (string.IsNullOrWhiteSpace(userAuthDTO.Password))
                {
                    return BadRequest(new { message = "Password is Required!" });
                }

                var authenticatedUser = await _authService.Auth(userAuthDTO.Email, userAuthDTO.Password);

                // Failed
                if (authenticatedUser == null)
                    return Unauthorized(new { message = "Invalid Email or Password." });

                // Set Cookies
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.UtcNow.AddMinutes(15),
                    Path = "/",
                    Domain = "localhost" 
                };

                Response.Cookies.Append("accessToken", authenticatedUser.AccessToken, cookieOptions);

                // Success
                return Ok(new
                {
                    id = authenticatedUser.Id,
                    email = authenticatedUser.Email,
                    accessToken = authenticatedUser.AccessToken,
                    refreshToken = authenticatedUser.RefreshToken
                });
            }
            catch (Exception error)
            {
                return StatusCode(500, error.Message);
            }
        }

        // LOGOUT a User Method
        // POST /api/Auth/logout
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            try
            {
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.UtcNow.AddDays(-1),
                    Path = "/",
                };

                Response.Cookies.Append("accessToken", "", cookieOptions);
                Response.Cookies.Delete("accessToken", cookieOptions);

                return Ok(new { message = "Logged Out Successfully!" });
            }
            catch (Exception error)
            {
                return StatusCode(500, error.Message);
            }
        }

        [HttpGet("validate-access-token")]
        public IActionResult ValidateAccessToken()
        {
            try
            {

                string? accessToken = null;

                var authHeader = Request.Headers["Authorization"].FirstOrDefault();
                if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
                {
                    accessToken = authHeader.Substring("Bearer ".Length).Trim();
                }

                if (string.IsNullOrEmpty(accessToken))
                {
                    accessToken = Request.Cookies["accessToken"];
                }

                if (string.IsNullOrEmpty(accessToken))
                {
                    return Unauthorized(new { valid = false, message = "Missing Token!" });
                }

                var claimsPrincipal = _tokenHelper.VerifyAccessToken(accessToken);

                if (claimsPrincipal == null)
                {
                    return Unauthorized(new { valid = false, message = "Invalid Token!" });
                }

                var id = claimsPrincipal.FindFirst("id")?.Value;
                var email = claimsPrincipal.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
                var role = claimsPrincipal.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

                return Ok(new
                {
                    valid = true,
                    tokenType = "accessToken",
                    id,
                    email,
                    role
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { valid = false, message = ex.Message });
            }
        }

        [HttpGet("validate-refresh-token")]
        public IActionResult ValidateRefreshToken()
        {
            try
            {
                // Read Refresh Token From Cookie
                var refreshToken = Request.Cookies["refreshToken"];

                if (string.IsNullOrEmpty(refreshToken))
                {
                    return Unauthorized(new { valid = false, message = "No Refresh Token Cookie Found." });
                }

                var claimsPrincipal = _tokenHelper.VerifyRefreshToken(refreshToken);

                if (claimsPrincipal == null)
                {
                    return Unauthorized(new { valid = false, message = "Invalid or Expired Refresh Token." });
                }

                // extract claims
                var id = claimsPrincipal.FindFirst("id")?.Value;
                var email = claimsPrincipal.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
                var role = claimsPrincipal.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

                return Ok(new
                {
                    valid = true,
                    tokenType = "refreshToken",
                    id,
                    email,
                    role
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { valid = false, message = ex.Message });
            }
        }

    }
}
