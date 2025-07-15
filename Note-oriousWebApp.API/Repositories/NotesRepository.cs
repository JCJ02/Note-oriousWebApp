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

        // CREATE a note method
        public async Task<NotesModel> Create(NotesModel note)
        {
            _context.Notes.Add(note);
            await _context.SaveChangesAsync();
            return note;
        }

        // GET notes method
        public async Task<List<NotesModel>> GetNotes()
        {
            return await _context.Notes
                .Where(notes => notes.DeletedAt == null)
                .ToListAsync();
        }

        // GET a note method
        public async Task<NotesModel?> GetNoteByID(int id)
        { 
            return await _context.Notes
                .FirstOrDefaultAsync(notes => notes.Id == id && notes.DeletedAt == null);
        }

        // GET a user method
        public async Task<UsersModel?> GetUserByID(int id)
        {
            return await _context.Users
                .FirstOrDefaultAsync(user => user.Id == id && user.DeletedAt == null);
        }
    }
}
