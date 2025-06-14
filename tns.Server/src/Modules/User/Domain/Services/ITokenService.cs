namespace tns.Server.src.Modules.User.Domain.Services
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
