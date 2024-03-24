using MediatR;
using OLAP.API.Infrastructure.BaseReuqestTypes;
using OLAP.API.Services;

namespace OLAP.API.Application.Behaviors
{
    public class AuthenticationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : BaseRequest<TResponse>
        //where TResponse : ResponseResult, new()
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IIdentityService _identityService;

        public AuthenticationBehavior(IHttpContextAccessor httpContextAccessor, IIdentityService identityService)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var teamId = _identityService.GetTeam();
            var userId = _identityService.GetIdentity();
            request.SetRequestId(Guid.NewGuid());
            request.SetTeam(teamId);
            request.SetUserId(userId);
            return await next();
        }
    }
}
