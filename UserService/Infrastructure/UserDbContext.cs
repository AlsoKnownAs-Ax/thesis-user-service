using UserService.Domain;
using Microsoft.EntityFrameworkCore;

namespace UserService.Infrastructure;

public class UserDbContext : DbContext
{
    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) {}

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
           entity.HasKey(u => u.Id);
           entity.HasIndex(u => u.Email).IsUnique();

           entity.Property(u => u.Email).IsRequired();
           entity.Property(u => u.FirstName).IsRequired();
           entity.Property(u => u.LastName).IsRequired(); 
        });
    }
}