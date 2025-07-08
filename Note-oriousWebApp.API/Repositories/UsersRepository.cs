using Microsoft.EntityFrameworkCore;
using Note_oriousWebApp.API.Configurations;
using Note_oriousWebApp.API.Models;

namespace Note_oriousWebApp.API.Repositories
{
    public class UsersRepository
    {
        // Call the AppDBContext to interact with the database
        private readonly AppDBContext _context;

        // Constructor that injects the application's DbContext
        public UsersRepository(AppDBContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // Save a new user and their account to the database
        public async Task<UsersModel> Create(UsersModel user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        // Get all users that are not soft-deleted (DeletedAt is null)
        public async Task<List<UsersModel>> GetAllUsers()
        {
            return await _context.Users
                .Where(users => users.DeletedAt == null) // Filter out soft-deleted users
                .ToListAsync(); // Execute the query and return the list
        }

        // Get a specific user by their ID (only if not soft-deleted)
        public async Task<UsersModel?> GetUserByID(int id)
        {
            return await _context.Users
                .FirstOrDefaultAsync(users => users.Id == id && users.DeletedAt == null); // Get user if not deleted
        }
    }
}
