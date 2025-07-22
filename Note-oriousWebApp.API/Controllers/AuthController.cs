using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Note_oriousWebApp.API.DTOs.UsersDTOs;
using Note_oriousWebApp.API.Helpers;
using Note_oriousWebApp.API.Services;

namespace Note_oriousWebApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // This Sets the Base Endpoint to: api/Auth
    public class AuthController : ControllerBase
    {
        // Call the UsersService Class
        private readonly AuthService _authService;

        // Constructor
        public AuthController(AuthService authService)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
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

                // Success
                return Ok(authenticatedUser);
            }
            catch (Exception error)
            {
                return StatusCode(500, error.Message);
            }
        }
    }
}
