using System.ComponentModel.DataAnnotations.Schema;

namespace JTM.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public byte[] PasswordHash { get; set; } = new byte[32];
        public byte[] PasswordSalt { get; set; } = new byte[32];
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime TokenCreated { get; set; }
        public DateTime TokenExpires { get; set; }
        public bool EmailConfirmed { get; set; }
        public string? ActivationToken { get; set; }
        public DateTime ActivationTokenExpires { get; set; }
        public bool Banned { get; set; }

        public List<WorkingTime> WorkingTimes { get; set; }
    }
}
