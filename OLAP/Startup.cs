using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OLAP.API.Infrastructure.Contexts.Identity;
using OLAP.API.Infrastructure.Options;
using OLAP.API.Infrastructure.AutoMapperExtensions;
using Serilog;
using Newtonsoft.Json.Converters;
using OLAP.API.Infrastructure.Filters;
using Asp.Versioning;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Logging;
using OLAP.API.Models.Identity;
using System.Net;
using OLAP.API.Certificate;
using OLAP.API.Application.Authentication;
using OLAP.API.Services;
using OLAP.API.Infrastructure.Repositories;
using MediatR;
using System.Reflection;
using FluentValidation;
using OLAP.API.Application.Behaviors;
using GenericRepository.Services;
using OLAP.API.Infrastructure.Contexts.Data;
using OLAP.API.Infrastructure.Middleware;
using OLAP.API.Managers;

namespace OLAP.API
{
    public class Startup
    {
        private readonly IWebHostEnvironment _environment;
        private readonly bool _isDevelopmentOrStaging;
        internal IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            //Configuration = configuration;
            _environment = env;
            _isDevelopmentOrStaging = env.IsDevelopment();

            Configuration = configuration;
            //StaticConfig = Configuration;

            //ServiceInfo = new ServiceInfo(serviceDisplayName: "Integration API",
            //                              serviceName: "Integration.API",
            //                              swaggerUIClientId: "integrationapi"
            //audience: IdentityConfiguration.IntegrationApiResource.Name,
            /*trustedAudience: IdentityConfiguration.IntegrationApiResource.Name*///);

            //ServiceInfo.RequiredScopes.Add((IdentityConfiguration.IntegrationApiScope.Name, IdentityConfiguration.IntegrationApiScope.DisplayName));
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var mvcCoreBuilder = services.AddMvcCore();

            services.AddApiVersioning(v =>
            {
                v.AssumeDefaultVersionWhenUnspecified = true;
                v.DefaultApiVersion = new Asp.Versioning.ApiVersion(1.0);
                v.ReportApiVersions = true;
                v.ApiVersionReader = ApiVersionReader.Combine(
                    new QueryStringApiVersionReader("api-version"),
                    new HeaderApiVersionReader("x-version"),
                    new MediaTypeApiVersionReader("ver"));
            }).AddApiExplorer(v =>
            {
                v.GroupNameFormat = "'v'vvv";
                v.SubstituteApiVersionInUrl = true;
            });

            var mvcBuilder = services.AddMvc(option =>
            {
                //option.OutputFormatters.Insert(0, new PowerAppOutputFormater());

                option.EnableEndpointRouting = false;

                option.Filters.Add<CommandValidationExceptionFilter>();
            });

            mvcBuilder.AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
            });

            services.AddAutoMapper();

            services.AddOptions();
            services.Configure<DBConnectionOptions>(Configuration.GetSection(DBConnectionOptions.SectionName));
            services.Configure<IdentityJWTOptions>(Configuration.GetSection(IdentityJWTOptions.SectionName));

            var dbConnections = services.BuildServiceProvider().GetRequiredService<IOptions<DBConnectionOptions>>().Value;
            services.AddDbContext<DataContext>(options =>
            {
                options.EnableSensitiveDataLogging();
                options.UseSqlServer(dbConnections.MainConnectionString,
                                     sqlServerOptionsAction: sqlOptions =>
                                     {
                                         sqlOptions.MigrationsAssembly(this.GetType().Assembly.GetName().Name);
                                         //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
                                         // sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(300), errorNumbersToAdd: null);
                                         sqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", DataContext.Schema);
                                     });
            }, ServiceLifetime.Scoped);

            services.AddDbContext<ApplicationContext>(options =>
            {
                options.EnableSensitiveDataLogging();
                options.UseSqlServer(dbConnections.MainConnectionString,
                                     sqlServerOptionsAction: sqlOptions =>
                                     {
                                         sqlOptions.MigrationsAssembly(this.GetType().Assembly.GetName().Name);
                                         //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
                                         // sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(300), errorNumbersToAdd: null);
                                         sqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", ApplicationContext.Schema);
                                     });
            }, ServiceLifetime.Scoped);

            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork<DataContext>));

            services.AddCors(o =>
            {
                o.AddPolicy("EnableCors", x => x.SetIsOriginAllowed(origin => true)
                                                .AllowAnyMethod()
                                                .AllowAnyHeader()
                                                .AllowCredentials());
            });

            ConfigureIdentity(services, dbConnections);


            services.AddTransient<IUserDataRepository, UserDataRepository>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddTransient<IDataManager, DataManager>();
            services.AddTransient<ITokenGenerationService, TokenGenerationService>();

            AddMediator(services);
        }

        private void ConfigureIdentity(IServiceCollection services, DBConnectionOptions dbConnections)
        {
            var identityOptions = services.BuildServiceProvider().GetRequiredService<IOptions<IdentityJWTOptions>>();

            var cerManager = new CertificateManager(identityOptions);
            services.AddSingleton(cerManager);

            var identity = services.AddIdentity<ApplicationUser, IdentityRole>(config =>
            {
                config.Password.RequireDigit = identityOptions.Value.Password.RequireDigit;
                config.Password.RequireLowercase = identityOptions.Value.Password.RequireLowercase;
                config.Password.RequireUppercase = identityOptions.Value.Password.RequireUppercase;
                config.Password.RequireNonAlphanumeric = identityOptions.Value.Password.RequireNonAlphanumeric;
                config.Password.RequiredLength = identityOptions.Value.Password.RequiredLength;
                config.Lockout.DefaultLockoutTimeSpan = identityOptions.Value.Lockout.DefaultLockoutTimeSpan;
                config.Lockout.MaxFailedAccessAttempts = identityOptions.Value.Lockout.MaxFailedAccessAttempts;

            });
            identity.AddEntityFrameworkStores<ApplicationContext>();
            identity.AddDefaultTokenProviders();
            //identity.AddDefaultUI();

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.MinimumSameSitePolicy = SameSiteMode.None;
                options.CheckConsentNeeded = context => true;
            });

            //services.AddTransient<IProfileService, ProfileService>();
            //services.Configure<AuthMessageSenderOptions>(Configuration);


            IdentityModelEventSource.ShowPII = true;
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
            | SecurityProtocolType.Tls11
            | SecurityProtocolType.Tls12;
            services.ConfigureAuthentication();
            //this.ServiceInfo.TrustedAudience,
            //hubUrls: this.ServiceInfo.HubURLs?.Where(x => x.isAuthoried).Select(y => y.hubUrl).ToList());

            services.ConfigureAuthorization();
        }

        private void AddMediator(IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthenticationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));
        }

        public void Configure(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var accContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
                accContext.Database.Migrate();

                var context = scope.ServiceProvider.GetRequiredService<DataContext>();
                context.Database.Migrate();
            }

            app.UseMiddleware<ExceptionMiddleware>();
            app.InitializeDatabase();

            app.UseSerilogRequestLogging();

            // Configure the HTTP request pipeline.
            //if (_isDevelopmentOrStaging)//app.Environment.IsDevelopment()
            //{
            //    app.UseSwagger();
            //    app.UseSwaggerUI();
            //}

            app.UseCors("EnableCors");
            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapRazorPages();
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllers();
                // endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}");
                //endpoints.MapControllerRoute(name: "default", pattern: "api/v{api-version:apiVersion}/{controller=Home}/{action=Index}");
                //endpoints.MapControllerRoute(name: "default", pattern: "api/v{api-version:apiVersion}/{controller=Account}/{action=Login}");
            });
        }
    }
}
