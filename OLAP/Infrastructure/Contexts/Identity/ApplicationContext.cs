using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OLAP.API.Models.Identity;

namespace OLAP.API.Infrastructure.Contexts.Identity
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser>
    {
        public static string Schema { get; private set; } = "Identity";

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema(Schema);

            builder.Entity<ApplicationUser>()
                   .Property(x => x.Id)
                   .HasMaxLength(36);

            builder.Entity<ApplicationUser>()
                   .Property(x => x.PartnerId)
                   .IsRequired(false)
                   .HasMaxLength(36);

            builder.Entity<ApplicationUser>()
                   .Property(x => x.FirstName)
                   .HasMaxLength(100);

            builder.Entity<ApplicationUser>()
                   .Property(x => x.LastName)
                   .HasMaxLength(100)
                   .IsRequired(false);

            builder.Entity<IdentityUserToken<string>>()
                   .Property(x => x.UserId)
                   .HasMaxLength(36);

            base.OnModelCreating(builder);
        }
    }
}
