using System.Text.Json.Serialization;

namespace Note_oriousWebApp.API.Models
{
    public class NotesModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DeletedAt { get; set; }
        public int UserId { get; set; }

        [JsonIgnore]
        public UsersModel User { get; set; } = null!;
    }
}
