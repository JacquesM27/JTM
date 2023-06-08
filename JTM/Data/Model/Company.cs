namespace JTM.Data.Model
{
    public class Company : IEntityBase
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public virtual ICollection<WorkingTime>? WorkingTimes { get; set; }
    }
}
