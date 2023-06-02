namespace JTM.DTO.WorkingTime
{
    public class DetailsWorkingTimeDto
    {
        public int Id { get; set; }
        public DateTime StartOdWorkingDate { get; set; }
        public DateTime EndOdWorkingDate { get; set; }
        public string Note { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
    }
}
