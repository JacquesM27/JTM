namespace JTM.DTO.Account
{
    public sealed record ConfirmDto
    {
        public int UserId { get; init; }
        public string Token { get; init; }

        public ConfirmDto(int userId, string token)
        {
            UserId = userId;
            Token = token;
        }
    }
}
