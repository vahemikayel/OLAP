namespace OLAP.API.Infrastructure.Options
{
    /// <summary>
    /// JWT configs
    /// </summary>
    public class JwtOptions
    {
        public static readonly string SectionName = "JWT";

        public string Secret { get; set; }
    }
}
