namespace JTM.DTO.WorkingTime
{
    public class WorkingTimeDto
    {
        public int Minutes { get; set; }
        public DateTime WorkingTime { get; set; }
        public string? Note { get; set; }
        public int EmployeeId { get; set; }
        public int? CompanyId { get; set; }
    }
}
