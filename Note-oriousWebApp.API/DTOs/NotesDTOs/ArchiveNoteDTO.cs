namespace Note_oriousWebApp.API.DTOs.NotesDTOs
{
    public class ArchiveNoteDTO
    {
        public bool IsAchive { get; set; } = true;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
