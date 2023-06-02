using System.ComponentModel.DataAnnotations.Schema;

namespace JTM.Model
{
    public class WorkingTime
    {
        public int Id { get; set; }
        public DateTime StartOdWorkingDate { get; set; }
        public DateTime EndOdWorkingDate { get; set; }
        public string Note { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool Deleted { get; set; }

        public int EmployeeId { get; set; }
        [ForeignKey(nameof(EmployeeId))]
        public User Employee { get; set; }

        public int? CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; }

        public int AuthorId { get; set; }
        [ForeignKey(nameof(AuthorId))]
        public User Author { get; set; }

        public int LastEditorId { get; set; }
        [ForeignKey(nameof(LastEditorId))]
        public User LastEditor { get; set; }
    }
}
