using Note_oriousWebApp.API.DTOs.UsersDTOs;
using Note_oriousWebApp.API.Models;
using Note_oriousWebApp.API.Repositories;
using Note_oriousWebApp.API.Helpers;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Note_oriousWebApp.API.Services
{
    public class UsersService
    {
        // Call the UsersRepository Class
        private readonly UsersRepository _usersRepository;

        // Contructor
        public UsersService(UsersRepository usersRepository)
        {
            _usersRepository = usersRepository ?? throw new ArgumentNullException(nameof(usersRepository));
        }

        // CREATE a user method
        public async Task<UserResponseDTO> Create(CreateUserDTO createUserDTO)
        {
            // Check if email already exist
            bool emailExists = await _usersRepository.IsEmailExisting(createUserDTO.Email);
            if (emailExists)
            {
                throw new Exception("Email Already Exists!");
            }


            // Hash the plain text password before saving it
            string hashedPassword = PasswordHelper.HashPassword(createUserDTO.Password);

            // Map the DTO to the UsersModel and associated AccountsModel
            var createUser = new UsersModel
            {
                Firstname = createUserDTO.Firstname,
                Lastname = createUserDTO.Lastname,
                Email = createUserDTO.Email,
                Account = new AccountsModel
                {
                    Password = hashedPassword,
                }
            };

            var createdUser = await _usersRepository.Create(createUser);

            return new UserResponseDTO
            {
                Id = createdUser.Id,
                Firstname = createdUser.Firstname,
                Lastname = createdUser.Lastname,
                Email = createdUser.Email,
                Role = createdUser.Role,
                CreatedAt = createdUser.CreatedAt,
                UpdatedAt = createdUser.UpdatedAt,
                DeletedAt = createdUser.DeletedAt
            };
        }

        // GET all users method
        public async Task<List<UserResponseDTO>> GetAllUsers()
        {
            var getAllUsers = await _usersRepository.GetAllUsers();

            // If no users found, throw an exception
            if (getAllUsers == null || getAllUsers.Count == 0)
            {
                throw new Exception("Users not Found!");
            }

            return getAllUsers.Select(getAllUsers => new UserResponseDTO
            {
                Id = getAllUsers.Id,
                Firstname = getAllUsers.Firstname,
                Lastname = getAllUsers.Lastname,
                Email = getAllUsers.Email,
                Role = getAllUsers.Role,
                CreatedAt = getAllUsers.CreatedAt,
                UpdatedAt = getAllUsers.UpdatedAt,
                DeletedAt = getAllUsers.DeletedAt
            }).ToList();
        }

        // GET a user method
        public async Task<UserResponseDTO?> GetUserByID(int id)
        {
            var getUser = await _usersRepository.GetUserByID(id);

            // Throw exception if user not found
            if (getUser == null)
            {
                throw new Exception($"User not Found with ID: {id}");
            }

            return new UserResponseDTO
            {
                Id = getUser.Id,
                Firstname = getUser.Firstname,
                Lastname = getUser.Lastname,
                Email = getUser.Email,
                Role = getUser.Role,
                CreatedAt = getUser.CreatedAt,
                UpdatedAt = getUser.UpdatedAt,
                DeletedAt = getUser.DeletedAt
            }; ;
        }

        // UPDATE a user method
        public async Task<UserResponseDTO> Update(int id, UpdateUserDTO user)
        {
            var isUserExisting = await _usersRepository.GetUserByID(id);
            if (isUserExisting == null)
            {
                throw new Exception("User not Found!");
            }

            isUserExisting.Firstname = user.Firstname;
            isUserExisting.Lastname = user.Lastname;
            isUserExisting.Email = user.Email;

            var updateUser = await _usersRepository.Update(isUserExisting);

            return new UserResponseDTO
            {
                Id = updateUser.Id,
                Firstname = updateUser.Firstname,
                Lastname = updateUser.Lastname,
                Email = updateUser.Email,
                Role = updateUser.Role,
                CreatedAt = updateUser.CreatedAt,
                UpdatedAt = updateUser.UpdatedAt,
                DeletedAt = updateUser.DeletedAt
            };

        }
    }
}
