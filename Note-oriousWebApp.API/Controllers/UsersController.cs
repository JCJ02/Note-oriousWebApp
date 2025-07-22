using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Note_oriousWebApp.API.DTOs.UsersDTOs;
using Note_oriousWebApp.API.Helpers;
using Note_oriousWebApp.API.Services;

namespace Note_oriousWebApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // This sets the base endpoint to: api/users
    public class UsersController : ControllerBase
    {
        // Call the UsersService Class
        private readonly UsersService _usersService;

        // Constructor
        public UsersController(UsersService usersService)
        {
            _usersService = usersService ?? throw new ArgumentNullException(nameof(usersService));
        }

        // CREATE a User Method
        // POST /api/Users
        //[AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserDTO createUserDTO)
        {
            try
            {
                // Basic input validation for required fields
                if (string.IsNullOrWhiteSpace(createUserDTO.Firstname))
                {
                    return BadRequest(new { message = "Firstname is Required." });
                }
                else if (string.IsNullOrWhiteSpace(createUserDTO.Lastname))
                {
                    return BadRequest(new { message = "Lastname is Required." });
                }
                else if (string.IsNullOrWhiteSpace(createUserDTO.Email))
                {
                    return BadRequest(new { message = "Email is Required." });
                }
                else if (!ValidationHelper.IsValidEmail(createUserDTO.Email))
                {
                    return BadRequest(new { message = "Invalid Email Address!" });
                }
                else if (string.IsNullOrWhiteSpace(createUserDTO.Password))
                {
                    return BadRequest(new { message = "Password is Required." });
                }

                // Call the Service to Create the User
                var createUser = await _usersService.Create(createUserDTO);

                // Return 201 Created Response with User Details and a Location Header
                return CreatedAtAction(nameof(GetUserByID), new { id = createUser.Id }, createUser);
            }
            catch (Exception error)
            {
                return StatusCode(500, error.Message);
            }
        }

        // GET All Users Method
        // GET /api/Users
        //[Authorize(Roles = "User")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _usersService.GetAllUsers();
                if (users == null || users.Count == 0)
                    return NotFound("Users not Found!");
                return Ok(users);
            }
            catch (Exception error)
            {
                return StatusCode(500, error.Message);
            }
        }

        /// GET a User Method
        // GET /api/Users/{id}
        //[Authorize(Roles = "User")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserByID(int id)
        {
            try
            {
                var user = await _usersService.GetUserByID(id);
                if (user == null)
                    return BadRequest("User not Found!");

                return Ok(user);
            }
            catch (Exception error)
            {
                return StatusCode(500, error.Message);
            }
        }

        // UPDATE a User Method
        // PUT /api/Users/{id}
        //[Authorize(Roles = "User")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUserDTO updateUserDTO)
        {
            try
            {
                // Basic input validation for required fields
                if (string.IsNullOrWhiteSpace(updateUserDTO.Firstname))
                {
                    return BadRequest("Firstname is Required!");
                }
                else if (string.IsNullOrWhiteSpace(updateUserDTO.Lastname))
                {
                    return BadRequest("Lastname is Required!");
                }
                else if (string.IsNullOrWhiteSpace(updateUserDTO.Email))
                {
                    return BadRequest("Email is Required!");
                }
                else if (!ValidationHelper.IsValidEmail(updateUserDTO.Email))
                {
                    return BadRequest("Invalid Email Address!");
                }

                var updatedUser = await _usersService.Update(id, updateUserDTO);
                return Ok(updatedUser);

            }
            catch (Exception error)
            {
                return StatusCode(500, error.Message);
            }
        }

        // SOFT-DELETE a User Method
        // DELETE /api/Users/{id}
        //[Authorize(Roles = "User")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDelete(int id, [FromBody] SoftDeleteUserDTO softDeleteUserDTO)
        {
            try
            {
                var softDeletedUser = await _usersService.SoftDelete(id, softDeleteUserDTO);
                return Ok($"User {id} is now Deleted at {softDeletedUser.DeletedAt}.");
            }
            catch (Exception error)
            {
                return StatusCode(500, error.Message);
            }
        }

        // AUTHENTICATE/LOGIN a User Method
        // POST /api/Users/
        //[HttpPost("Auth")]
        //public async Task<IActionResult> Auth([FromBody] UserAuthDTO userAuthDTO)
        //{
        //    try
        //    {
        //        if (string.IsNullOrWhiteSpace(userAuthDTO.Email))
        //        {
        //            return BadRequest(new { message = "Email is Required!" });
        //        }
        //        else if (!ValidationHelper.IsValidEmail(userAuthDTO.Email))
        //        {
        //            return BadRequest(new { message = "Invalid Email Address!" });
        //        }
        //        else if (string.IsNullOrWhiteSpace(userAuthDTO.Password))
        //        {
        //            return BadRequest(new { message = "Password is Required!" });
        //        }

        //        var authenticatedUser = await _usersService.Auth(userAuthDTO.Email, userAuthDTO.Password);

        //        // Failed
        //        if (authenticatedUser == null)
        //            return Unauthorized(new { message = "Invalid Email or Password." });

        //        // Success
        //        return Ok(authenticatedUser);
        //    }
        //    catch (Exception error)
        //    {
        //        return StatusCode(500, error.Message);
        //    }
        //}

    }
}
