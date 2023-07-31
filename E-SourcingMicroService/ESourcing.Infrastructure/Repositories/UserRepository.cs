using ESourcing.Core.Entities;
using ESourcing.Infrastructure.Data;
using ESourcing.Infrastructure.Repositories.Base;

namespace ESourcing.Infrastructure.Repositories
{
    public class UserRepository : Repository<AppUser>, IUserRepository
    {
        public UserRepository(WebAppContext dbContext):base(dbContext)
        {

        }
    }
}
