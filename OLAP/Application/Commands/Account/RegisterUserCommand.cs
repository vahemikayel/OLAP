using AutoMapper;
using MediatR;
using OLAP.API.Exceptions;
using OLAP.API.Infrastructure.BaseReuqestTypes;
using OLAP.API.Models.Identity;
using OLAP.API.Services;

namespace OLAP.API.Application.Commands.Account
{
    public class RegisterUserCommand : BaseRequest<bool>
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserPass { get; set; }

        public string[] Roles { get; set; }
    }

    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, bool>
    {
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;

        public RegisterUserCommandHandler(IAccountService accountService, IMapper mapper)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
        }

        public async Task<bool> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<ApplicationUser>(request);

            var result = await _accountService.CreateAsync(user, request.UserPass);

            if (!result.Errors.Any())
            {
                var systemRols = await _accountService.GetSystemRolsAsync();
                foreach (var item in request.Roles)
                {
                    if (item.Any(x => systemRols.Any(r => r.Name.ToLower().Trim() == item.ToLower().Trim())))
                        await _accountService.AddToRoleAsync(user, item);
                }
            }
            if (!result.Succeeded)
                throw new AppException(result.Errors);

            throw new NotImplementedException();
        }
    }
}
