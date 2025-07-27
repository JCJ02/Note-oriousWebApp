namespace Note_oriousWebApp.API.DTOs.NotesDTOs
{
    public class NoteResponseDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public bool IsArchive { get; set; }
        public DateTime? Reminder { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public int UserID { get; set; } 
    }
}
