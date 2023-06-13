using JTM.Data.Model;

namespace JTM.Data.Repository.CompanyRepo
{
    public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
    {
        public CompanyRepository(DataContext context) : base(context)
        {
        }
    }
}
