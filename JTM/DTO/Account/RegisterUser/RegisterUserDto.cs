namespace JTM.DTO.Account.RegisterUser
{
    public sealed record RegisterUserDto
    {
        public string UserName { get; init; }
        public string Email { get; init; }
        public string EmailConfirmation { get; init; }
        public string Password { get; init; }
        public string PasswordConfirmation { get; init; }

        public RegisterUserDto(string userName, string email, string emailConfirmation, string password, string passwordConfirmation)
        {
            UserName = userName;
            Email = email;
            EmailConfirmation = emailConfirmation;
            Password = password;
            PasswordConfirmation = passwordConfirmation;
        }
    }
}
