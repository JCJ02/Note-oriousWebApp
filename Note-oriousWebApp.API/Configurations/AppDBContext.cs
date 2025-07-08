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
        public DbSet<UsersModel> Users { get; set; }
        public DbSet<AccountsModel> Accounts { get; set; }
        public DbSet<RolesModel> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UsersModel>()
                .HasOne(user => user.Account)
                .WithOne(account => account.User)
                .HasForeignKey<AccountsModel>(account => account.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
