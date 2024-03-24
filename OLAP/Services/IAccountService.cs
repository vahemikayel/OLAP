using Microsoft.AspNetCore.Identity;
using OLAP.API.Application.Commands.Account;
using OLAP.API.Models.Identity;
using OLAP.API.Models.Response;

namespace OLAP.API.Services
{
    public interface IAccountService
    {
        Task<IdentityResult> CreateAsync(ApplicationUser user, string password);

        Task<IdentityResult> AddToRolesAsync(ApplicationUser user, IEnumerable<string> roles);

        Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role);

        Task<List<IdentityRole>> GetSystemRolsAsync();

        Task<List<ApplicationUserResponseModel>> GetUsersAsync(CancellationToken cancellationToken = default);
        Task<bool> IsSameUserNameExist(string userName, CancellationToken cancellationToken);
        Task<bool> IsSameEmailExist(string email, CancellationToken cancellationToken);
        Task<bool> IsPartnerExist(Guid partnerId, CancellationToken cancellationToken);

        Task<(SignInResult SignInResult, AccessTokenResponseModel TokenResponse)> LoginByEMail(LoginCommand model, CancellationToken cancellationToken);

        Task<IdentityResult> DeleteUser(Guid userId);

        string GenerateTempPassword(string prefix, string userName);

        string GenerateUserName(string prefix, string userName);
    }
}
