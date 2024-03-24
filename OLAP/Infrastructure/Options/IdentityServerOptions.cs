namespace OLAP.API.Infrastructure.Options
{
    public class IdentityServerOptions
    {
        public static readonly string SectionName = "IdentityServer";

        public InputLengthRestrictionsOptions InputLengthRestrictions { get; set; }
    }
}
