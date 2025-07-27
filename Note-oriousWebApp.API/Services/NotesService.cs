using Microsoft.EntityFrameworkCore;
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
        public async Task<NotesModel> Create(int userId, CreateNoteDTO createNoteDTO)
        {
            var isUserExisting = await _notesRepository.GetUserByID(userId);
            if (isUserExisting == null)
                return null;

            var note = new NotesModel
            {
                Title = createNoteDTO.Title,
                Content = createNoteDTO.Content,
                UserId = userId
            };

            return await _notesRepository.Create(note);
        }

        // // GET Notes By User ID Method
        public async Task<List<NotesModel>> GetNotes(int userId)
        {
            var getAllNotes = await _notesRepository.GetNotes(userId);

            // Return Empty Lists
            return getAllNotes ?? new List<NotesModel>(); 
        }

        // GET Archive Notes Method
        public async Task<List<NotesModel>> GetArchiveNotes(int userId)
        {
            var getArchiveNotes = await _notesRepository.GetArchiveNotes(userId);

            // Return Empty Lists
            return getArchiveNotes ?? new List<NotesModel>();
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
            if (isNoteExisting == null)
                return null;

            isNoteExisting.Title = updatedNoteDTO.Title;
            isNoteExisting.Content = updatedNoteDTO.Content;
            isNoteExisting.UpdatedAt = DateTime.UtcNow;

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
        public async Task<NoteResponseDTO> SoftDelete(int id)
        {
            var isNoteExisting = await _notesRepository.GetNoteByID(id);
            if (isNoteExisting == null)
                return null;

            isNoteExisting.UpdatedAt = DateTime.UtcNow;
            isNoteExisting.DeletedAt = DateTime.UtcNow;

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

        // DELETE a Note Metho
        public async Task<bool> Delete(int id)
        {
            var isNoteExisting = await _notesRepository.GetNoteByID(id);
            if (isNoteExisting == null) 
                return false;

            await _notesRepository.Delete(id);
            return true;
        }

        // GET Soft-Deleted Notes Method
        public async Task<List<NotesModel>> GetSoftDeletedNotes(int userId)
        {
            var isSoftDeletedNoteExisting = await _notesRepository.GetSoftDeletedNotes(userId);

            // Return Soft-Deleted Lists
            return isSoftDeletedNoteExisting ?? new List<NotesModel>();
        }

        // ARCHIVE a Note Method
        public async Task<NoteResponseDTO> ArchiveNote(int id, ArchiveNoteDTO archiveNoteDTO)
        {
            var isNoteExisting = await _notesRepository.GetNoteByID(id);
            if (isNoteExisting == null)
                return null;

            isNoteExisting.UpdatedAt= archiveNoteDTO.UpdatedAt;
            isNoteExisting.IsArchive = archiveNoteDTO.IsAchive;

            var archiveNote = await _notesRepository.ArchiveNote(isNoteExisting);

            return new NoteResponseDTO
            {
                Id = archiveNote.Id,
                Title = archiveNote.Title,
                Content = archiveNote.Content,
                IsArchive = archiveNote.IsArchive,
                Reminder = archiveNote.Reminder,
                CreatedAt = archiveNote.CreatedAt,
                UpdatedAt = archiveNote.UpdatedAt,
                DeletedAt = archiveNote.DeletedAt,
                UserID = archiveNote.UserId
            };
        }

    }
}
