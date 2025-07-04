using Microsoft.EntityFrameworkCore;
using Note_oriousWebApp.API.Configurations;
using Note_oriousWebApp.API.Models;
using System;

namespace Note_oriousWebApp.API.Repositories
{
    // This class is responsible for direct access to the Notes table in the database
    public class NotesRepository
    {
        private readonly AppDBContext _context;

        // Constructor injects the application's DbContext
        public NotesRepository(AppDBContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // Save a new note to the database
        public async Task<NotesModel> Create(NotesModel note)
        {
            _context.Notes.Add(note);
            await _context.SaveChangesAsync();
            return note;
        }

        // Get all notes that are not deleted (soft delete filter)
        public async Task<List<NotesModel>> GetAllNotes()
        {
            return await _context.Notes
                .Where(notes => notes.DeletedAt == null)
                .ToListAsync();
        }

        // Get a specific note by ID (only if not soft-deleted)
        public async Task<NotesModel?> GetNoteByID(int id)
        { 
            return await _context.Notes
                .FirstOrDefaultAsync(notes => notes.Id == id && notes.DeletedAt == null);
        }
    }
}
