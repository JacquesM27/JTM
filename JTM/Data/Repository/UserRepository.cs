using JTM.Data;
using JTM.Data.Model;

namespace JTM.Data.Repository
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(DataContext context) : base(context) { }
    }
}
