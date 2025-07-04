using Note_oriousWebApp.API.Models;
using Note_oriousWebApp.API.Repositories;

namespace Note_oriousWebApp.API.Services
{
    // This class handles business rules and logic before interacting with the repository
    public class NotesService
    {
        private readonly NotesRepository _notesRepository;

        public NotesService(NotesRepository notesRepository)
        {
            _notesRepository = notesRepository ?? throw new ArgumentNullException(nameof(notesRepository));
        }

        // Create a new note with business rules (like setting timestamps)
        public async Task<NotesModel> Create(string title, string content)
        {
            var note = new NotesModel
            {
                Title = title,
                Content = content,
            };

            return await _notesRepository.Create(note);
        }

        // Get all notes (calls repository)
        public async Task<List<NotesModel>> GetAllNotes()
        {
            var getAllNotes = await _notesRepository.GetAllNotes();

            if (getAllNotes == null || getAllNotes.Count == 0)
            {
                throw new Exception("No Notes Found!");
            }

            return getAllNotes;
        }

        // Get a single note by ID (calls repository)
        public async Task<NotesModel?> GetNoteByID(int id)
        {
            var getNote = await _notesRepository.GetNoteByID(id);

            if (getNote == null)
            {
                throw new Exception($"No Note Found with ID: {id}");
            }

            return getNote;
        }
    }
}
