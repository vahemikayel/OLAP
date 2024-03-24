using Microsoft.AspNetCore.Identity;
using OLAP.API.Infrastructure.Contexts.Identity;
using OLAP.API.Models.Identity;

namespace OLAP.API.Application.Authentication
{
    /// <summary>
    /// First Time initialization
    /// </summary>
    public static class IdentityServerDatabaseInitialization
    {
        /// <summary>
        /// Initialize Admin user
        /// </summary>
        /// <param name="app"></param>
        public static void InitializeDatabase(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices
                                         .GetService<IServiceScopeFactory>()
                                         .CreateScope())
            {
                SeedUserData(serviceScope);
            }
        }

        private static void SeedUserData(IServiceScope serviceScope)
        {
            var context = serviceScope
                              .ServiceProvider
                              .GetRequiredService<ApplicationContext>();

            var userManager = serviceScope
                              .ServiceProvider
                              .GetRequiredService<UserManager<ApplicationUser>>();

            var roleManager = serviceScope
                              .ServiceProvider
                              .GetRequiredService<RoleManager<IdentityRole>>();


            if (!context.Roles.Any())
            {
                roleManager.CreateAsync(new IdentityRole(Roles.Admin)).GetAwaiter().GetResult();
                roleManager.CreateAsync(new IdentityRole(Roles.ManagePartner)).GetAwaiter().GetResult();
                roleManager.CreateAsync(new IdentityRole(Roles.ActivatePartner)).GetAwaiter().GetResult();
                roleManager.CreateAsync(new IdentityRole(Roles.Partner)).GetAwaiter().GetResult();
                roleManager.CreateAsync(new IdentityRole(Roles.PartnerUser)).GetAwaiter().GetResult();
            }

            if (!context.Users.Any())
            {
                var user = new ApplicationUser()
                {
                    UserName = "Admin",
                    FirstName = "Administrator"
                };

                var result = userManager.CreateAsync(user, "Admin!234").GetAwaiter().GetResult();

                if (result.Errors.Count() <= 0)
                {
                    userManager.AddToRoleAsync(user, Roles.Admin).GetAwaiter().GetResult();
                    userManager.AddToRoleAsync(user, Roles.ManagePartner).GetAwaiter().GetResult();
                    userManager.AddToRoleAsync(user, Roles.ActivatePartner).GetAwaiter().GetResult();
                    userManager.AddToRoleAsync(user, Roles.Partner).GetAwaiter().GetResult();
                    userManager.AddToRoleAsync(user, Roles.PartnerUser).GetAwaiter().GetResult();
                }
            }
        }
    }
}
