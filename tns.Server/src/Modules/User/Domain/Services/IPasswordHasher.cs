namespace tns.Server.src.Modules.User.Domain.Services
{
    public interface IPasswordHasher
    {
        string GenerateSalt();
        string HashPassword(string password, string salt);
        bool VerifyPassword(string password, string hash, string salt);
    }
}
