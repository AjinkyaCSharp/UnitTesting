using Microsoft.EntityFrameworkCore;

namespace UnitTestDemo.Model
{
    public class AccountsDbContext:DbContext
    {
        public AccountsDbContext(DbContextOptions<AccountsDbContext> options):base(options) { }
     
        public DbSet<Account> Accounts { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>().ToTable("Accounts");   
        }
    }
}