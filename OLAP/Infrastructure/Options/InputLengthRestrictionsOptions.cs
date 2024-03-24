namespace OLAP.API.Infrastructure.Options
{
    public class InputLengthRestrictionsOptions
    {
        public static readonly string SectionName = "InputLengthRestrictions";

        public int Scope { get; set; }

        public int Jwt { get; set; }

        public int TokenHandle { get; set; }
    }
}
