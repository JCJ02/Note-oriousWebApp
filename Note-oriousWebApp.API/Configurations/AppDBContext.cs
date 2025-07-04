using Microsoft.EntityFrameworkCore;
using Note_oriousWebApp.API.Models;

namespace Note_oriousWebApp.API.Configurations
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) 
        {
        }

        // DbSet for NotesModel or Tables
        public DbSet<NotesModel> Notes { get; set; }
    }
}
