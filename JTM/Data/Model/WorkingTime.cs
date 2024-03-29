﻿namespace JTM.Data.Model
{
    public class WorkingTime : IEntityBase
    {
        public int Id { get; set; }
        public DateTime WorkingDate { get; set; }
        public int SecondsOfWork { get; set; }
        public string Note { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime LastModified { get; set; }
        public bool Deleted { get; set; }

        public int? CompanyId { get; set; }
        public Company? Company { get; set; }

        public int EmployeeId { get; set; }
        public User Employee { get; set; }

        public int AuthorId { get; set; }
        public virtual User Author { get; set; }

        public int LastEditorId { get; set; }
        public virtual User LastEditor { get; set; }
    }
}
