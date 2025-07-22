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

        // GET Exisiting Email Method
        public async Task<bool> IsEmailExisting(string email)
        {
            return await _context.Users
                .AnyAsync(user => user.Email == email && user.DeletedAt == null);
        }

        // CREATE a User and Acount Method
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

        // GET All Users Method
        public async Task<List<UsersModel>> GetAllUsers()
        {
            return await _context.Users
                .Where(users => users.DeletedAt == null)
                .ToListAsync();
        }

        // GET a User Method
        public async Task<UsersModel?> GetUserByID(int id)
        {
            return await _context.Users
                .Where(user => user.Id == id && user.DeletedAt == null)
                .FirstOrDefaultAsync();
        }

        // GET the User, Account and Note Method
        public async Task<UsersModel?> GetUserAndAccountByID(int id)
        {
            return await _context.Users
                .Where(user => user.Id == id && user.DeletedAt == null)
                .Include(user => user.Account)
                .Include(user => user.Notes)
                .FirstOrDefaultAsync();
        }

        // UPDATE a User Method
        public async Task<UsersModel> Update(UsersModel user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        // SOFT-DELETE a User method
        public async Task<UsersModel> SoftDelete(UsersModel user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        // AUTHENTICATION/LOGIN User Method
        //public async Task<UsersModel?> Auth(string email)
        //{
        //    return await _context.Users
        //        .Where(user => user.Email == email && user.DeletedAt == null)
        //        .Include(user => user.Account)
        //        .FirstOrDefaultAsync();
        //}
    }
}
