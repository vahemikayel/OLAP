using System.Text.RegularExpressions;

namespace OLAP.API.Extensions
{
    public static class GuidExtension
    {
        public static bool MatchToGuid(this string guid)
        {
            Match m = Regex.Match(guid, @"[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}", RegexOptions.IgnoreCase);

            return m.Success;
        }
    }
}
