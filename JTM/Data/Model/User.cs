﻿namespace JTM.Data.Model
{
    public class User : IEntityBase
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Role { get; set; } = "user";
        public byte[] PasswordHash { get; set; } = new byte[32];
        public byte[] PasswordSalt { get; set; } = new byte[32];
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime TokenCreated { get; set; }
        public DateTime TokenExpires { get; set; }
        public bool EmailConfirmed { get; set; } = false;
        public string? ActivationToken { get; set; }
        public DateTime ActivationTokenExpires { get; set; }
        public string? PasswordResetToken { get; set; }
        public DateTime PasswordTokenExpires { get; set; }
        public bool Banned { get; set; } = false;

        public virtual ICollection<WorkingTime> WorkingTimes { get; set; }
        public virtual ICollection<WorkingTime> AuthorOfWorkingTimes { get; set; }
        public virtual ICollection<WorkingTime> LastEditorOfWorkingTimes { get; set; }
    }
}
