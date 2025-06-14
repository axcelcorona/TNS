using System.Security.Cryptography;
using tns.Server.src.Modules.User.Domain.Services;

namespace tns.Server.src.Modules.User.Infrastructure.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        public string GenerateSalt()
        {
            byte[] salt = new byte[128 / 8];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(salt);
            return Convert.ToBase64String(salt);
        }

        public string HashPassword(string password, string salt)
        {
            var saltBytes = Convert.FromBase64String(salt);
            using var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 10000, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(256 / 8);
            return Convert.ToBase64String(hash);
        }

        public bool VerifyPassword(string password, string hash, string salt)
        {
            var newHash = HashPassword(password, salt);
            return newHash == hash;
        }
    }
}
