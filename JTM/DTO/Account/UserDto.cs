namespace JTM.DTO.Account
{
    public record UserDto
    {
        public string Email { get; init; }
        public string Password { get; init; }

        public UserDto(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
