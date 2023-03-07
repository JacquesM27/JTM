namespace JTM.Model
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public List<WorkingTime> WorkingTimes { get; set; }
    }
}
