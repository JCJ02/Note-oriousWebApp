using Microsoft.EntityFrameworkCore;
using Note_oriousWebApp.API.Configurations;
using Note_oriousWebApp.API.Models;
using System;

namespace Note_oriousWebApp.API.Repositories
{
    public class NotesRepository
    {
        private readonly AppDBContext _context;

        // Constructor
        public NotesRepository(AppDBContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // CREATE a Note Method
        public async Task<NotesModel> Create(NotesModel note)
        {
            _context.Notes.Add(note);
            await _context.SaveChangesAsync();
            return note;
        }

        // GET Notes Method
        public async Task<List<NotesModel>> GetNotes()
        {
            return await _context.Notes
                .Where(notes => notes.DeletedAt == null)
                .ToListAsync();
        }

        // GET a Note Method
        public async Task<NotesModel?> GetNoteByID(int id)
        { 
            return await _context.Notes
                .Where(notes => notes.Id == id && notes.DeletedAt == null)
                .FirstOrDefaultAsync();
        }

        // GET a User Method
        public async Task<UsersModel?> GetUserByID(int id)
        {
            return await _context.Users
                .Where(user => user.Id == id && user.DeletedAt == null)
                .FirstOrDefaultAsync();
        }

        // UPDATE a Note Method
        public async Task<NotesModel> Update(NotesModel notes)
        {
            _context.Notes.Update(notes);
            await _context.SaveChangesAsync();
            return notes;
        }

        // SOFT-DELETE a Note Method
        public async Task<NotesModel> SoftDelete(NotesModel notes)
        {
            _context.Notes.Update(notes);
            await _context.SaveChangesAsync();
            return notes;
        }

    }
}
