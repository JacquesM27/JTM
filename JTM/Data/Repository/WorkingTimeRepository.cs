using JTM.Data.Model;

namespace JTM.Data.Repository
{
    public class WorkingTimeRepository : RepositoryBase<WorkingTime>, IWorkingTimeRepository
    {
        public WorkingTimeRepository(DataContext context) : base(context) { }
    }
}
