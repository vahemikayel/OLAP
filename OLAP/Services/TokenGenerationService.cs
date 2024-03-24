using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OLAP.API.Infrastructure.Options;
using OLAP.API.Models.Identity;
using OLAP.API.Models.Response;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OLAP.API.Services
{
    internal class TokenGenerationService : ITokenGenerationService
    {
        private readonly IOptions<IdentityJWTOptions> _options;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly TokenValidationParameters _tokenValidationParams;

        public TokenGenerationService(IOptions<IdentityJWTOptions> options, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, TokenValidationParameters tokenValidationParams)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenValidationParams = tokenValidationParams;
        }

        public async Task<AccessTokenResponseModel> GenerateAccessTokenAsync(ApplicationUser user, string clientId, List<string> scopes = null, CancellationToken cancellationToken = default)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_options.Value.JWT.Secret);

            var claims = await GetAllValidClaims(user);
            if (!string.IsNullOrWhiteSpace(clientId))
                claims.Add(new Claim(IIdentityService.ClientKey, clientId));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddYears(1), // 5-10 
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);

            return new AccessTokenResponseModel()
            {
                Token = jwtToken,
                Success = true,
                //RefreshToken = refreshToken.Token
            };
        }

        private async Task<List<Claim>> GetAllValidClaims(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim("Id", user.Id),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            if (!string.IsNullOrWhiteSpace(user.Email))
                claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));

            // Getting the claims that we have assigned to the user
            var userClaims = await _userManager.GetClaimsAsync(user);
            claims.AddRange(userClaims);

            // Get the user role and add it to the claims
            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var userRole in userRoles)
            {
                var role = await _roleManager.FindByNameAsync(userRole);

                if (role != null)
                {
                    claims.Add(new Claim(ClaimTypes.Role, userRole));

                    var roleClaims = await _roleManager.GetClaimsAsync(role);
                    claims.AddRange(roleClaims);
                }
            }

            return claims;
        }
    }
}
