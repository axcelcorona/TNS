namespace tns.Server.src.Shared.Authenticacion
{
    public class BasicAuthConfig
    {
        public List<CredentialBasicAuth> Credentials { get; set; } = new List<CredentialBasicAuth>();
    }

    public class CredentialBasicAuth
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
