using Note_oriousWebApp.API.DTOs.UsersDTOs;
using Note_oriousWebApp.API.Helpers;
using Note_oriousWebApp.API.Repositories;
using System.Security.Claims;

namespace Note_oriousWebApp.API.Services
{
    public class AuthService
    {
        // Call the AuthRepository Class
        private readonly AuthRepository _authRepository;
        private readonly TokenHelper _tokenHelper;

        // Contructor
        public AuthService(AuthRepository authRepository, TokenHelper tokenHelper)
        {
            _authRepository = authRepository ?? throw new ArgumentNullException(nameof(authRepository));
            _tokenHelper = tokenHelper;
        }

        // LOGIN a User Method
        public async Task<UserAuthResponseDTO> Auth(string email, string password)
        {
            var user = await _authRepository.Auth(email);
            if (user == null)
                return null;

            var isPasswordValid = PasswordHelper.VerifyPassword(password, user.Account.Password);
            if (!isPasswordValid)
                return null;

            var payload = new Dictionary<string, string>
            {
                { "id", user.Id.ToString() },
                { "email", user.Email },
                { ClaimTypes.Role, user.Role }
            };

            var accessToken = _tokenHelper.GenerateAccessToken(payload);
            var refreshToken = _tokenHelper.GenerateRefreshToken(payload);

            return new UserAuthResponseDTO
            {
                Id = user.Id,
                Email = user.Email,
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

    }
}
