namespace JTM.DTO.WorkingTime
{
    public class WorkingTimeDto
    {
        public DateTime WorkingDate { get; set; }
        public int Seconds { get; set; }
        public string? Note { get; set; }
        public int EmployeeId { get; set; }
        public int? CompanyId { get; set; }
    }
}
