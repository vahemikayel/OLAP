using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OLAP.API.Models.Entity.Data;
using OLAP.API.Models.Identity;

namespace OLAP.API.Infrastructure.Contexts.Data
{
    public class DataContext : DbContext
    {
        internal static string Schema { get; private set; } = "Data";

        public DataContext()
        {
            
        }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
                
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema(Schema);

            builder.Entity<CountryEModel>(Configure);
            builder.Entity<IndicatorEModel>(Configure);
            builder.Entity<DataEModel>(Configure);
            //builder.Entity<TeamEModel>(ConfigureTeam);
            //builder.Entity<ApplicationEModel>(ConfigureApplication);
            //builder.Entity<ApplicationMethodEModel>(ConfigureApplicationMethod);
            //builder.Entity<TeamApplicationEModel>(ConfigureTeamApplication);
            //builder.Entity<TeamApplicationMethodEModel>(ConfigureTeamApplicationMethod);
            //builder.Entity<TeamApplicationMethodUserEModel>(ConfigureTeamApplicationMethodUser);
            // builder.Entity<ApplicationUser>(Configure);
            //builder.Entity<IntegrationLogEModel>(ConfigureIntegrationLog);

            base.OnModelCreating(builder);
        }

        private void Configure(EntityTypeBuilder<IndicatorEModel> builder)
        {
            builder.ToTable("Indicators")
                   .HasKey(x => x.Id);

            builder.Property(x => x.IndicatorName)
                  .HasMaxLength(250);

            builder.Property(x => x.IndicatorCode)
                   .HasMaxLength(50);
        }

        private void Configure(EntityTypeBuilder<CountryEModel> builder)
        {
            builder.ToTable("Countries")
                   .HasKey(x => x.Id);

            builder.Property(x => x.CountryName)
                   .HasMaxLength(250);

            builder.Property(x => x.CountryCode)
                   .HasMaxLength(10);
        }

        private void Configure(EntityTypeBuilder<DataEModel> builder)
        {
            builder.ToTable("Data")
                   .HasKey(x => x.Id);

            builder.Property(x => x.CountryId)
                   .IsRequired();

            builder.HasOne(x => x.Country)
                   .WithMany(x => x.DataItems)
                   .HasForeignKey(x => x.CountryId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.IndicatorId)
                   .IsRequired();

            builder.HasOne(x => x.Indicator)
                   .WithMany(x => x.DataItems)
                   .HasForeignKey(x => x.IndicatorId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.Frequency)
                   .HasMaxLength(10);

            builder.Property(x => x.Date)
                   //.HasColumnType("datetime2")
                   .HasMaxLength(10);

            //builder.Property(x => x.Value)
            //       .HasColumnType("float");
        }

        private void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasKey(x => x.Id);
            builder.ToView("AspNetUsers", "Identity");
        }
    }
}
