namespace JTM.DTO
{
    public class ChangePasswordDto
    {
        public int UserId { get; set; }
        public string Password { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}
