using OLAP.API.Models.Identity;

namespace OLAP.API.Infrastructure.Repositories
{
    public interface IUserDataRepository
    {
        Task CreateUser(ApplicationUser user);
    }
}
