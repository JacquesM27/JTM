using JTM.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace JTM.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<WorkingTime> WorkingTimes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<WorkingTime>()
                .HasOne(e => e.Company)
                .WithMany(e => e.WorkingTimes)
                .HasForeignKey(e => e.CompanyId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<WorkingTime>()
                .HasOne(e => e.Employee)
                .WithMany(e => e.WorkingTimes)
                .HasForeignKey(e => e.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WorkingTime>()
                .HasOne(e => e.Author)
                .WithMany(e => e.AuthorOfWorkingTimes)
                .HasForeignKey(e => e.AuthorId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<WorkingTime>()
                .HasOne(e => e.LastEditor)
                .WithMany(e => e.LastEditorOfWorkingTimes)
                .HasForeignKey(e => e.LastEditorId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
