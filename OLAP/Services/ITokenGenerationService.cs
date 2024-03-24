using OLAP.API.Models.Identity;
using OLAP.API.Models.Response;

namespace OLAP.API.Services
{
    public interface ITokenGenerationService
    {
        Task<AccessTokenResponseModel> GenerateAccessTokenAsync(ApplicationUser user, string clientId, List<string> scopes = null, CancellationToken cancellationToken = default);
    }
}
