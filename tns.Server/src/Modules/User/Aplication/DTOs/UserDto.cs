namespace tns.Server.src.Modules.User.Aplication.DTOs
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public UserDto(Guid id, string name, string email)
        {
            Id = id;
            Name = name;
            Email = email;
        }
    }

    public class UserDetailsDto : UserDto
    {
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public UserDetailsDto(Guid id, string name, string email) : base(id, name, email){}
    }
}
