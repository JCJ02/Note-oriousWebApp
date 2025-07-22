using Note_oriousWebApp.API.DTOs.Notes;
using Note_oriousWebApp.API.DTOs.NotesDTOs;
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

        // CREATE a Note Method
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

        // GET Notes Method
        public async Task<List<NotesModel>> GetNotes()
        {
            var getAllNotes = await _notesRepository.GetNotes();

            if (getAllNotes == null || getAllNotes.Count == 0)
            {
                throw new Exception("No Notes Found!");
            }

            return getAllNotes;
        }

        // GET a Note Method
        public async Task<NotesModel?> GetNoteByID(int id)
        {
            var getNote = await _notesRepository.GetNoteByID(id);

            if (getNote == null)
            {
                throw new Exception($"No Note Found with ID: {id}");
            }

            return getNote;
        }

        // UPDATE a Note Method
        public async Task<NoteResponseDTO> Update(int id, UpdateNoteDTO updatedNoteDTO)
        {
            var isNoteExisting = await _notesRepository.GetNoteByID(id);
            if(isNoteExisting == null)
            {
                throw new Exception("Note not Found!");
            }

            isNoteExisting.Title = updatedNoteDTO.Title;
            isNoteExisting.Content = updatedNoteDTO.Content;
            isNoteExisting.UpdatedAt = updatedNoteDTO.UpdatedAt;

            var updatedNote = await _notesRepository.Update(isNoteExisting);

            return new NoteResponseDTO
            {
                Id = updatedNote.Id,
                Title = updatedNote.Title,
                Content = updatedNote.Content,
                CreatedAt = updatedNote.CreatedAt,
                UpdatedAt = updatedNote.UpdatedAt,
                DeletedAt = updatedNote.DeletedAt,
                UserID = updatedNote.UserId
            };

        }

        // SOFT-DELETE a Note Method
        public async Task<NoteResponseDTO> SoftDelete(int id, SoftDeleteNoteDTO softDeleteNoteDTO)
        {
            var isNoteExisting = await _notesRepository.GetNoteByID(id);
            if(isNoteExisting == null)
            {
                throw new Exception("Note not Found!");
            }

            isNoteExisting.UpdatedAt = softDeleteNoteDTO.UpdatedAt;
            isNoteExisting.DeletedAt = softDeleteNoteDTO.DeletedAt;

            var softDeletedNote = await _notesRepository.SoftDelete(isNoteExisting);

            return new NoteResponseDTO
            {
                Id = softDeletedNote.Id,
                Title = softDeletedNote.Title,
                Content = softDeletedNote.Content,
                CreatedAt = softDeletedNote.CreatedAt,
                UpdatedAt = softDeletedNote.UpdatedAt,
                DeletedAt = softDeletedNote.DeletedAt,
                UserID = softDeletedNote.UserId
            };
        }

    }
}
