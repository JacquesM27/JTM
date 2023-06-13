using JTM.Data.Model;

namespace JTM.Data.Repository.WorkingTimeRepo
{
    public class WorkingTimeRepository : RepositoryBase<WorkingTime>, IWorkingTimeRepository
    {
        public WorkingTimeRepository(DataContext context) : base(context) { }
    }
}
