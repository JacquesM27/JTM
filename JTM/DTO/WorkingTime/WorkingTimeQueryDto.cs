namespace JTM.DTO.WorkingTime
{
    public sealed record WorkingTimeQueryDto
    {
        public IEnumerable<int>? EmployeesId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public IEnumerable<int>? CompaniesId { get; set; }
        public string? NoteSearchPhrase { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
