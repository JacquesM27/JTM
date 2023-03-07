using Microsoft.EntityFrameworkCore;

namespace JTM.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }


    }
}
