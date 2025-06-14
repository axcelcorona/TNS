
using Microsoft.EntityFrameworkCore;

namespace tns.Server.src.Modules.User.Infrastructure.Repositories
{
    public class UserRepository : Domain.Repositories.IUserRepository
    {

        private readonly Data.UserDbContext _context;

        public UserRepository(Data.UserDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<bool> ExistsAsync(Guid id) => await _context.Users.AnyAsync(u => u.Id == id);

        public async Task<bool> ExistsByEmailAsync(string email) => await _context.Users.AnyAsync(u => u.Email == email);

        public async Task<Domain.User?> GetByEmailAsync(string email) => await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

        public async Task<Domain.User?> GetByIdAsync(Guid id) => await _context.Users.FindAsync(id);

        public async Task AddAsync(Domain.User user)
        {
            _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Domain.User user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }


        public async Task UpdateAsync(Domain.User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
