namespace tns.Server.src.Modules.User.Domain
{
    public class User
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public string Salt { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        
        private User()
        {
        }

        public User(Guid id, string name, string email, string passwordHash, string salt, DateTime createdAt, DateTime? updatedAt)
        {
            Id = id;
            Name = name;
            Email = email;
            PasswordHash = passwordHash;
            Salt = salt;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        public void Update(string name, string email)
        {
            Name = name;
            Email = email;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
