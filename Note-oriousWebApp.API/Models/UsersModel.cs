namespace Note_oriousWebApp.API.Models
{
    public class UsersModel
    {
        public int Id { get; set; }
        public string Firstname { get; set; } = string.Empty;
        public string Lastname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = "User";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DeletedAt { get; set; }
        public AccountsModel Account { get; set; } = null!;
        public ICollection<NotesModel> Notes { get; set; } = new List<NotesModel>();
    }
}
