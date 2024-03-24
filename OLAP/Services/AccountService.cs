using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OLAP.API.Application.Authentication;
using OLAP.API.Application.Commands.Account;
using OLAP.API.Infrastructure.Repositories;
using OLAP.API.Models.Identity;
using OLAP.API.Models.Response;

namespace OLAP.API.Services
{
    public class AccountService : IAccountService
    {
        private readonly IPasswordHasher<ApplicationUser> _hasher;
        private readonly IUserDataRepository _userDataRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly ITokenGenerationService _tokenGenerationService;

        public AccountService(IUserDataRepository userDataRepository,
                              IPasswordHasher<ApplicationUser> hasher,
                              UserManager<ApplicationUser> userManager,
                              RoleManager<IdentityRole> roleManager,
                              IMapper mapper,
                              SignInManager<ApplicationUser> signInManager,
                              ITokenGenerationService tokenGenerationService)
        {
            _userDataRepository = userDataRepository ?? throw new ArgumentNullException(nameof(userDataRepository));
            _hasher = hasher;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _tokenGenerationService = tokenGenerationService ?? throw new ArgumentNullException(nameof(tokenGenerationService));
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> CreateAsync(ApplicationUser user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<IdentityResult> AddToRolesAsync(ApplicationUser user, IEnumerable<string> roles)
        {
            return await _userManager.AddToRolesAsync(user, roles);
        }

        public async Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role)
        {
            return await _userManager.AddToRoleAsync(user, role);
        }

        public async Task<List<IdentityRole>> GetSystemRolsAsync()
        {
            return await _roleManager.Roles.ToListAsync();
        }

        public async Task<IdentityResult> DeleteUser(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var userRoles = await _userManager.GetRolesAsync(user);
            var res = await _userManager.RemoveFromRolesAsync(user, userRoles);
            if (!res.Succeeded)
                return res;
            return await _userManager.DeleteAsync(user);
        }

        public async Task<List<ApplicationUserResponseModel>> GetUsersAsync(CancellationToken cancellationToken = default)
        {
            var res = await _userManager.Users.ToListAsync(cancellationToken);
            return _mapper.Map<List<ApplicationUserResponseModel>>(res);
        }

        public async Task<bool> IsSameUserNameExist(string userName, CancellationToken cancellationToken)
        {
            userName = userName.Trim().ToLower();
            var res = await _userManager.Users.AnyAsync(x => x.UserName.ToLower() == userName);
            return res;
        }

        public async Task<bool> IsSameEmailExist(string email, CancellationToken cancellationToken)
        {
            email = email.Trim().ToLower();
            var res = await _userManager.Users.AnyAsync(x => x.Email.ToLower() == email);
            return res;
        }

        public async Task<bool> IsPartnerExist(Guid partnerId, CancellationToken cancellationToken)
        {
            var res = await _userManager.Users.AnyAsync(x => x.Id == partnerId.ToString());
            return res;
        }

        public async Task<(SignInResult SignInResult, AccessTokenResponseModel TokenResponse)> LoginByEMail(LoginCommand model, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(model.Email);
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberLogin, lockoutOnFailure: false);
            if (!result.Succeeded)
                return (result, null);

            var token = await _tokenGenerationService.GenerateAccessTokenAsync(user, "", null, cancellationToken: cancellationToken);
            return (result, token);
        }

        public string GenerateTempPassword(string prefix, string userName)
        {
            return $"{prefix.ToUpper()}#{userName}Temp1";
        }

        public string GenerateUserName(string prefix, string userName)
        {
            return $"{prefix}.{userName}";
        }

        private static string NormalizePhoneNumber(string phone)
        {
            return new string(phone.Where(Char.IsDigit).ToArray());
        }

        private string NormalizeUsername(string username)
        {
            return username.Trim().ToUpperInvariant();
        }
    }
}
