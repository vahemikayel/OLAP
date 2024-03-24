namespace OLAP.API.Infrastructure.Options
{
    public class DBConnectionOptions
    {
        public static readonly string SectionName = "ConnectionStrings";

        public string MainConnectionString { get; set; }
    }
}
