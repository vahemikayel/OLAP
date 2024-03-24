using Microsoft.EntityFrameworkCore;
using OLAP.API.Infrastructure.Contexts.Identity;
using OLAP.API.Models.Identity;

namespace OLAP.API.Infrastructure.Repositories
{
    public class UserDataRepository : IUserDataRepository
    {
        private readonly ApplicationContext _dbContext;

        public UserDataRepository(ApplicationContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task CreateUser(ApplicationUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            await _dbContext.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            _dbContext.Entry(user).State = EntityState.Detached;
        }
    }
}
