using MediatR;
using Microsoft.AspNetCore.Identity;
using OLAP.API.Infrastructure.BaseReuqestTypes;
using OLAP.API.Models.Identity;
using OLAP.API.Models.Response;
using OLAP.API.Services;

namespace OLAP.API.Application.Commands.Account
{
    public class LoginCommand : BaseHttpRequest<LoginResponseModel>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool RememberLogin { get; set; }
    }

    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponseModel>
    {
        private readonly ITokenGenerationService _tokenGenerationService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public LoginCommandHandler(ITokenGenerationService tokenGenerationService,
                                   SignInManager<ApplicationUser> signInManager,
                                   UserManager<ApplicationUser> userManager)
        {
            _tokenGenerationService = tokenGenerationService ?? throw new ArgumentNullException(nameof(tokenGenerationService));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _userManager = userManager;
        }

        public async Task<LoginResponseModel> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.Email);
            var result = await _signInManager.PasswordSignInAsync(request.Email, request.Password, request.RememberLogin, lockoutOnFailure: false);
            if (!result.Succeeded)
                return new LoginResponseModel(result);

            var token = await _tokenGenerationService.GenerateAccessTokenAsync(user, user.Id, cancellationToken: cancellationToken);
            return new LoginResponseModel(result, token);
        }
    }
}
