using Note_oriousWebApp.API.DTOs.UsersDTOs;
using Note_oriousWebApp.API.Models;
using Note_oriousWebApp.API.Repositories;
using Note_oriousWebApp.API.Helpers;

namespace Note_oriousWebApp.API.Services
{
    public class UsersService
    {
        // Call the UsersRepository Class
        private readonly UsersRepository _usersRepository;
        private readonly TokenHelper _tokenHelper;

        // Contructor
        public UsersService(UsersRepository usersRepository, TokenHelper tokenHelper)
        {
            _usersRepository = usersRepository ?? throw new ArgumentNullException(nameof(usersRepository));
            _tokenHelper = tokenHelper;
        }

        // CREATE a User Method
        public async Task<UserResponseDTO> Create(CreateUserDTO createUserDTO)
        {
            // Check if Email already exist
            bool emailExists = await _usersRepository.IsEmailExisting(createUserDTO.Email);
            if (emailExists)
            {
                throw new Exception("Email Already Exists!");
            }

            // Hash the plain text Password before saving it
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

        // GET all Users Method
        public async Task<List<UserResponseDTO>> GetAllUsers()
        {
            var getAllUsers = await _usersRepository.GetAllUsers();

            // If no Users found, throw an exception
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

        // GET a User Method
        public async Task<UserResponseDTO?> GetUserByID(int id)
        {
            var getUser = await _usersRepository.GetUserByID(id);

            // Throw exception if User not found
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

        // UPDATE a User Method
        public async Task<UserResponseDTO> Update(int id, UpdateUserDTO updateUserDTO)
        {
            var isUserExisting = await _usersRepository.GetUserByID(id);
            if (isUserExisting == null)
            {
                throw new Exception("User not Found!");
            }

            isUserExisting.Firstname = updateUserDTO.Firstname;
            isUserExisting.Lastname = updateUserDTO.Lastname;
            isUserExisting.Email = updateUserDTO.Email;
            isUserExisting.UpdatedAt = updateUserDTO.UpdatedAt;

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

        // SOFT-DELETE a User Method
        public async Task<UserResponseDTO> SoftDelete(int id, SoftDeleteUserDTO softDeleteUserDTO)
        {
            var isUserExisting = await _usersRepository.GetUserAndAccountByID(id);
            if (isUserExisting == null)
            {
                throw new Exception("User not Found!");
            }

            isUserExisting.UpdatedAt = softDeleteUserDTO.UpdatedAt;
            isUserExisting.DeletedAt = softDeleteUserDTO.DeletedAt;

            if (isUserExisting.Account != null)
            {
                isUserExisting.Account.DeletedAt = softDeleteUserDTO.DeletedAt;
            }

            // Soft-Delete All Notes of the Selected User
            if(isUserExisting.Notes != null && isUserExisting.Notes.Any())
            {
                foreach (var note in isUserExisting.Notes)
                {
                    note.DeletedAt = softDeleteUserDTO.DeletedAt;
                    note.UpdatedAt = softDeleteUserDTO.UpdatedAt;
                }
            }

            var softDeleteuser = await _usersRepository.SoftDelete(isUserExisting);

            return new UserResponseDTO
            {
                Id = softDeleteuser.Id,
                Firstname = softDeleteuser.Firstname,
                Lastname = softDeleteuser.Lastname,
                Email = softDeleteuser.Email,
                Role = softDeleteuser.Role,
                CreatedAt = softDeleteuser.CreatedAt,
                UpdatedAt = softDeleteuser.UpdatedAt,
                DeletedAt = softDeleteuser.DeletedAt
            };
        }

        // AUTHENTICATE/LOGIN a User Method
        //public async Task<UserAuthResponseDTO> Auth(string email, string password)
        //{
        //    var user = await _usersRepository.Auth(email);
        //    if (user == null)
        //        return null;

        //    var isPasswordValid = PasswordHelper.VerifyPassword(password, user.Account.Password);
        //    if (!isPasswordValid)
        //        return null;

        //    var payload = new Dictionary<string, string>
        //    {
        //        { "id", user.Id.ToString() },
        //        { "email", user.Email }
        //    };

        //    var accessToken = _tokenHelper.GenerateAccessToken(payload);
        //    var refreshToken = _tokenHelper.GenerateRefreshToken(payload);

        //    return new UserAuthResponseDTO
        //    {
        //        Id = user.Id,
        //        Email = user.Email,
        //        AccessToken = accessToken,
        //        RefreshToken = refreshToken
        //    };
        //}

    }
}
