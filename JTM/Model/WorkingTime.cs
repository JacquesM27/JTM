using System.ComponentModel.DataAnnotations.Schema;

namespace JTM.Model
{
    public class WorkingTime
    {
        public int Id { get; set; }
        public int Minutes { get; set; }
        public DateTime WorkingDate { get; set; }
        public string Note { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;


        public int EmployeeId { get; set; }
        [ForeignKey(nameof(EmployeeId))]
        public User User { get; set; }

        public int CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; }
    }
}
