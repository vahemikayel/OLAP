using System.Data;

namespace OLAP.API.Application.Authentication
{
    internal static class AuthorizationServiceCollectionExtensions
    {
        internal static IServiceCollection ConfigureAuthorization(this IServiceCollection services)
        {
            services.AddAuthorizationCore(options =>
            {
                options.AddPolicy(Policies.Admin, policy =>
                {
                    policy.RequireClaim("role", Roles.Admin);
                });

                options.AddPolicy(Policies.Management, policy =>
                {
                    policy.RequireClaim("role", Roles.ManagePartner, Roles.ActivatePartner);
                });

                options.AddPolicy(Policies.Partner, policy =>
                {
                    policy.RequireClaim("role", Roles.Partner, Roles.Admin);
                });

                options.AddPolicy(Policies.PartnerUser, policy =>
                {
                    policy.RequireClaim("role", Roles.PartnerUser, Policies.Partner, Policies.Admin);
                });
            });
            return services;
        }
    }
}
