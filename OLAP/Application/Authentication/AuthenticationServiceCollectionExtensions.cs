using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OLAP.API.Infrastructure.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OLAP.API.Application.Authentication
{
    internal static class AuthenticationServiceCollectionExtensions
    {
        private readonly static string _validIssuer = "lclHost";

        internal static AuthenticationBuilder ConfigureAuthentication(this IServiceCollection services,
                                                                    List<string> hubUrls = null)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            //var urlsOption = services.BuildServiceProvider().GetRequiredService<IOptions<UriOptions>>().Value;

            var identityConf = services.BuildServiceProvider().GetRequiredService<IOptions<IdentityJWTOptions>>().Value;
            var key = Encoding.ASCII.GetBytes(identityConf.JWT.Secret);
            var tokenValidationParams = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidIssuer = _validIssuer,
                ValidateAudience = false,
                //ValidAudience = _validIssuer,
                ValidateLifetime = true,
                RequireExpirationTime = false,
                ClockSkew = TimeSpan.Zero,
            };

            services.AddSingleton(tokenValidationParams);

            var authBuilder = services.AddAuthentication(o =>
            {
                o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;// CookieAuthenticationDefaults.AuthenticationScheme;
                //o.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;// IdentityConstants.ExternalScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            });
            
            authBuilder.AddJwtBearer(options =>
            {
                options.SaveToken = true;
                //options.Authority = _validIssuer;// identityUrl;
                options.RequireHttpsMetadata = false;
                //options.Audience = _validIssuer;
                options.ClaimsIssuer = _validIssuer;
                options.TokenValidationParameters = tokenValidationParams;
                options.Events = new JwtBearerEvents()
                {
                    OnForbidden = frb =>
                    {
                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = ctx =>
                    {
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        if (context.SecurityToken is JwtSecurityToken accessToken)
                        {
                            if (context.Principal.Identity is ClaimsIdentity identity)
                            {
                                identity.AddClaim(new Claim("access_token", accessToken.RawData));
                            }
                        }
                        return Task.CompletedTask;
                    },
                    OnMessageReceived = mr =>
                    {
                        var path = mr.HttpContext.Request.Path;

                        var accessToken = mr.Request.Headers["access_token"];
                        if (string.IsNullOrEmpty(accessToken))
                            accessToken = mr.Request.Query["access_token"];
                        if (!string.IsNullOrEmpty(accessToken))
                        {
                            mr.Token = accessToken;
                            if (hubUrls != null && hubUrls.Count > 0 && hubUrls.Any(x => "/" + x == path.Value))
                            {
                                return Task.CompletedTask;
                            }
                            mr.HttpContext.Request.Headers.TryAdd("Authorization", "Bearer " + accessToken);
                        }

                        return Task.CompletedTask;
                    },
                    OnChallenge = context =>
                    {
                        return Task.CompletedTask;
                    }
                };
            });

            authBuilder.AddCookie(options =>
            {
                options.Cookie.IsEssential = true;
            });

            return authBuilder;
        }
    }
}
