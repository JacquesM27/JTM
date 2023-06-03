namespace JTM.DTO.Account
{
    public record AuthResponseDto
    { 
        public string Token { get; init; }
        public string RefreshToken { get; init; }
        public DateTime TokenExpires { get; init; }

        public AuthResponseDto(string token, string refreshToken, DateTime tokenExpires)
        {
            Token = token;
            RefreshToken = refreshToken;
            TokenExpires = tokenExpires;
        }
    }
}
