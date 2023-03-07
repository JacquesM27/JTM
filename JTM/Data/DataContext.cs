using JTM.Model;
using Microsoft.EntityFrameworkCore;

namespace JTM.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<WorkingTime> WorkingTimes { get; set; }
    }
}
