namespace tns.Server.src.Modules.User.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByEmailAsync(string email); 
        Task AddAsync(User user); 
        Task UpdateAsync(User user); 
        Task DeleteAsync(User user);
        Task<bool> ExistsAsync(Guid id);
        Task<bool> ExistsByEmailAsync(string email);
    }
}
