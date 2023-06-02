namespace JTM.DTO.WorkingTime
{
    public class WorkingTimeQueryDto
    {
        public IEnumerable<int>? EmployeesId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? CompanyId { get; set; }
        public string? NoteSearchPhrase { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
