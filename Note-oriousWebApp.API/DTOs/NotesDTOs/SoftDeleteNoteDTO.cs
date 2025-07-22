namespace Note_oriousWebApp.API.DTOs.NotesDTOs
{
    public class SoftDeleteNoteDTO
    {
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public DateTime DeletedAt { get; set; } = DateTime.UtcNow;
    }
}
