using JTM.Data.Model;

namespace JTM.Data.Repository.UserRepo
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(DataContext context) : base(context) { }
    }
}
