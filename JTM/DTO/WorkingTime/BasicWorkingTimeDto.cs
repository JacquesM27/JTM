namespace JTM.DTO.WorkingTime
{
    public class BasicWorkingTimeDto
    {
        public int Id { get; set; }
        public DateTime StartOdWorkingDate { get; set; }
        public DateTime EndOdWorkingDate { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
    }
}
