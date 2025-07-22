using Microsoft.EntityFrameworkCore;
using Note_oriousWebApp.API.Configurations;
using Note_oriousWebApp.API.Models;

namespace Note_oriousWebApp.API.Repositories
{
    public class AuthRepository
    {
        // Call AppDBContext
        private readonly AppDBContext _context;

        // Constructor
        public AuthRepository(AppDBContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // LOGIN User Method
        public async Task<UsersModel?> Auth(string email)
        {
            return await _context.Users
                .Where(user => user.Email == email && user.DeletedAt == null)
                .Include(user => user.Account)
                .FirstOrDefaultAsync();
        }

    }
}
