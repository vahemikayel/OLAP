using Microsoft.AspNetCore.Identity;

namespace OLAP.API.Infrastructure.Options
{
    /// <summary>
    /// Identity configratuins
    /// </summary>
    public class IdentityJWTOptions
    {
        public static readonly string SectionName = "Identity";

        public JwtOptions JWT { get; set; }

        public CertificateOptions Certificate { get; set; }

        public PasswordOptions Password { get; set; }

        public LockoutOptions Lockout { get; set; }

        public IdentityServerOptions IdentityServer { get; set; }
    }
}
