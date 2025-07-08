using System.Text.Json.Serialization;

namespace Note_oriousWebApp.API.Models
{
    public class AccountsModel
    {
        public int Id { get; set; }
        public string Password { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DeletedAt { get; set; }

        // Foreign Key
        public int UserId { get; set; }

        // Navigation property (will be ignored during JSON serialization to prevent infinite loop)
        [JsonIgnore]
        public UsersModel User { get; set; } = null!;
    }
}
