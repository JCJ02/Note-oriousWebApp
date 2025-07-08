using Microsoft.AspNetCore.Mvc;
using Note_oriousWebApp.API.DTOs.UsersDTOs;
using Note_oriousWebApp.API.Services;

namespace Note_oriousWebApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // This sets the base endpoint to: api/users
    public class UsersController : ControllerBase
    {
        // Call the UsersService to handle user-related operations
        private readonly UsersService _usersService;

        // Inject the service into the controller
        public UsersController(UsersService usersService)
        {
            _usersService = usersService ?? throw new ArgumentNullException(nameof(usersService));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserDTO createUserDTO)
        {
            try
            {
                // Basic input validation for required fields
                if (string.IsNullOrWhiteSpace(createUserDTO.Firstname))
                {
                    return BadRequest("Firstname is Required!");
                }
                else if (string.IsNullOrWhiteSpace(createUserDTO.Lastname))
                {
                    return BadRequest("Lastname is Required!");
                }
                else if (string.IsNullOrWhiteSpace(createUserDTO.Email))
                {
                    return BadRequest("Email is Required!");
                }
                else if (string.IsNullOrWhiteSpace(createUserDTO.Password))
                {
                    return BadRequest("Password is Required!");
                }

                // Call the service to create the user
                var createUser = await _usersService.Create(createUserDTO);

                // Return 201 Created response with user details and a location header
                return CreatedAtAction(nameof(GetUserByID), new { id = createUser.Id }, createUser);
            }
            catch (Exception)
            {
                return StatusCode(500, "An Error Occurred!");
            }
        }

        // get /api/users
        // return all users
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
            catch (Exception)
            {
                // Log the exception (not implemented here)
                return StatusCode(500, "An Error Occurred!");
            }
        }

        // get /api/users/{id}
        // return a specific user by id
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
            catch (Exception)
            {
                // Log the exception (not implemented here)
                return StatusCode(500, "An Error Occurred!");
            }
        }
    }
}
