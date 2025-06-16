using Microsoft.EntityFrameworkCore;

namespace tns.Server.src.Modules.User.Infrastructure.Data
{
    public class UserDbContext : DbContext
    {
        public DbSet<Domain.User> Users { get; set; }

        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Domain.User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Name).IsRequired().HasMaxLength(100);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(255);
                entity.Property(u => u.PasswordHash).IsRequired().HasMaxLength(255);
                entity.Property(u => u.Salt).IsRequired().HasMaxLength(255);
                entity.Property(u => u.IsEmailConfirmed).IsRequired().HasDefaultValue(false);
                entity.Property(u => u.CreatedAt).IsRequired();
                entity.Property(u => u.UpdatedAt).HasColumnType("timestamp with time zone")
                                                 .HasConversion(
                                                    v => v.HasValue ? v.Value.ToUniversalTime() : v,
                                                    v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v
                                                 );
            });
        }
    }
}
