using Note_oriousWebApp.API.DTOs.Notes;
using Note_oriousWebApp.API.DTOs.UsersDTOs;
using Note_oriousWebApp.API.Models;
using Note_oriousWebApp.API.Repositories;

namespace Note_oriousWebApp.API.Services
{
    public class NotesService
    {
        // Call NotesRepository Class
        private readonly NotesRepository _notesRepository;

        // Contstructor
        public NotesService(NotesRepository notesRepository)
        {
            _notesRepository = notesRepository ?? throw new ArgumentNullException(nameof(notesRepository));
        }

        // CREATE a note method
        public async Task<NotesModel> Create(int id, CreateNoteDTO createNoteDTO)
        {
            var isUserExisting = await _notesRepository.GetUserByID(id);
            if (isUserExisting == null)
            {
                throw new Exception("User not Found!");
            }

            var note = new NotesModel
            {
                Title = createNoteDTO.Title,
                Content = createNoteDTO.Content,
                UserId = id
            };

            return await _notesRepository.Create(note);
        }

        // GET notes method
        public async Task<List<NotesModel>> GetNotes()
        {
            var getAllNotes = await _notesRepository.GetNotes();

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
