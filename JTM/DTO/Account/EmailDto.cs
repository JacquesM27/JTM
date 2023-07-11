namespace JTM.DTO.Account
{
    public sealed record EmailDto
    {
        public string Email { get; init; }

        public EmailDto(string email)
        {
            Email = email;
        }
    }
}
