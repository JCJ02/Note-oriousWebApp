namespace Note_oriousWebApp.API.DTOs.NotesDTOs
{
    public class UpdateNoteDTO
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
