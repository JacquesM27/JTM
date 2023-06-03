namespace JTM.DTO.Account
{
    public record ChangePasswordDto
    {
        public int UserId { get; init; }
        public string Password { get; init; }
        public string Token { get; init; }

        public ChangePasswordDto(int userId, string password, string token)
        {
            UserId = userId;
            Password = password;
            Token = token;
        }
    }
}
