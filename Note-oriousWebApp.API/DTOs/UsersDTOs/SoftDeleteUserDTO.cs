namespace Note_oriousWebApp.API.DTOs.UsersDTOs
{
    public class SoftDeleteUserDTO
    {
        public DateTime DeletedAt { get; set; } = DateTime.UtcNow;
    }
}
