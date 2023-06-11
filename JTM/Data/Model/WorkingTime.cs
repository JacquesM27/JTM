namespace JTM.Data.Model
{
    public class WorkingTime : IEntityBase
    {
        public int Id { get; set; }
        public DateTime StartOdWorkingDate { get; set; }
        public DateTime EndOdWorkingDate { get; set; }
        public string Note { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
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
