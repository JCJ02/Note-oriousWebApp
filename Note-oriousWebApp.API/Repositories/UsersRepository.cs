using Microsoft.EntityFrameworkCore;
using Note_oriousWebApp.API.Configurations;
using Note_oriousWebApp.API.Models;

namespace Note_oriousWebApp.API.Repositories
{
    public class UsersRepository
    {
        // Call AppDBContext
        private readonly AppDBContext _context;

        // Constructor
        public UsersRepository(AppDBContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // GET exisiting email address method
        public async Task<bool> IsEmailExisting(string email)
        {
            return await _context.Users
                .AnyAsync(user => user.Email == email && user.DeletedAt == null);
        }

        // CREATE a user + account methond
        public async Task<UsersModel> Create(UsersModel user)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                    return user;
                } catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        // GET all users method
        public async Task<List<UsersModel>> GetAllUsers()
        {
            return await _context.Users
                .Where(users => users.DeletedAt == null)
                .ToListAsync();
        }

        // GET a user method
        public async Task<UsersModel?> GetUserByID(int id)
        {
            return await _context.Users
                .FirstOrDefaultAsync(user => user.Id == id && user.DeletedAt == null);
        }

        // GET the user and account method
        public async Task<UsersModel?> GetUserAndAccountByID(int id)
        {
            return await _context.Users
                .Include(user => user.Account)
                .FirstOrDefaultAsync(user => user.Id == id && user.DeletedAt == null);
        }

        // UPDATE a user method
        public async Task<UsersModel> Update(UsersModel user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        // SOFT-DELETE a user method
        public async Task<UsersModel> SoftDelete(UsersModel user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }
    }
}
