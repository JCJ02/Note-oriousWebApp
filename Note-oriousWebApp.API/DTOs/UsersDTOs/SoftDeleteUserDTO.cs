namespace Note_oriousWebApp.API.DTOs.UsersDTOs
{
    public class SoftDeleteUserDTO
    {
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public DateTime DeletedAt { get; set; } = DateTime.UtcNow;
    }
}
