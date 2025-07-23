using Microsoft.AspNetCore.Authorization;
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
        //[AllowAnonymous]
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

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.Now.AddDays(7)
                };

                Response.Cookies.Append("refreshToken", authenticatedUser.RefreshToken, cookieOptions);

                // Success
                return Ok(new
                {
                    id = authenticatedUser.Id,
                    email = authenticatedUser.Email,
                    accessToken = authenticatedUser.AccessToken
                });
            }
            catch (Exception error)
            {
                return StatusCode(500, error.Message);
            }
        }

        //public async Task<IActionResult> accessToken()
        //{
        //    try
        //    {
        //        var authHeader = Request.Headers["Authorization"].FirstOrDefault();
        //        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
        //            return Unauthorized(new { messeage = "Missing or Invalid Authorization Header!" });

        //        var token = authHeader.Substring("Bearer ".Length).Trim();

        //        // Validate Token
        //        var claimsPrincipal = _tokenHelper.ValidateToken(token);
        //        if (claimsPrincipal == null)
        //            return Unauthorized(new { message = "Invalid Token" });

        //        var id = claimsPrincipal.FindFirst("id")?.Value;
        //        var email = claimsPrincipal.FindFirst("email")?.Value;
        //        var role = claimsPrincipal.FindFirst(ClaimTypes.Role)?.Value;

        //        return Ok(new
        //        {
        //            id = id,
        //            email = email,
        //            role = role
        //        });
        //    }
        //    catch (Exception error)
        //    {
        //        return StatusCode(500, error.Message);
        //    }
        //}

    }
}
