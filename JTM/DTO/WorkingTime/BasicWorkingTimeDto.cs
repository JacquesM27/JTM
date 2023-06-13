namespace JTM.DTO.WorkingTime
{
    public class BasicWorkingTimeDto
    {
        public int Id { get; set; }
        public DateTime WorkingDate { get; set; }
        public int SecondsOfWork { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
    }
}
