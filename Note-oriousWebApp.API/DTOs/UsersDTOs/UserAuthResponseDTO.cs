namespace Note_oriousWebApp.API.DTOs.UsersDTOs
{
    public class UserAuthResponseDTO
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}
