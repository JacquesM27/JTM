namespace JTM.DTO.WorkingTime
{
    public sealed record DetailsWorkingTimeDto
    {
        public int Id { get; set; }
        public DateTime WorkingDate { get; set; }
        public int SecondsOfWork { get; set; }
        public string Note { get; set; } = string.Empty;
        public string EmployeeName { get; set; } = string.Empty;
        public string AuthorName { get; set; } = string.Empty;
        public string LastEditorName { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
    }
}
