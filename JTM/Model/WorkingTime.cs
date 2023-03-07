namespace JTM.Model
{
    public class WorkingTime
    {
        public int Id { get; set; }
        public int Minutes { get; set; }
        public DateOnly WorkingDate { get; set; }
        public string Note { get; set; } = string.Empty;
    }
}
