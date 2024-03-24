using OLAP.API.Services;

namespace OLAP.API.Extensions
{
    public static class IdentityServiceHttpContextExtensions
    {
        public static Guid GetIdentity(this HttpContext httpContext)
        {
            string identity = GetUserIdentity(httpContext);
            if (string.IsNullOrEmpty(identity) || !Guid.TryParse(identity, out Guid identityGuid))
            {
                return Guid.Empty;
            }
            return identityGuid;
        }


        public static string GetUserIdentity(this HttpContext httpContext)
        {
            if (!httpContext.User.Identity.IsAuthenticated)
                return string.Empty;

            var u = httpContext.User?.FindFirst("sub")?.Value;
            if (!string.IsNullOrWhiteSpace(u) && u.MatchToGuid())
                return u;
            return string.Empty;
        }

        public static int GetUserType(this HttpContext httpContext)
        {
            return Convert.ToInt32(httpContext.User.FindFirst("type").Value);
        }

        public static string[] GetUserRoles(this HttpContext httpContext)
        {

            var roles = httpContext.User.FindAll("role").Select(x => x.Value).ToList();
            return roles.ToArray();
        }

        public static Uri GetAbsoluteUri(this HttpContext httpContext)
        {
            var request = httpContext.Request;
            var uriBuilder = new UriBuilder
            {
                Scheme = request.Scheme,
                Host = request.Host.Host,
                Path = request.Path.ToString(),
                Query = request.QueryString.ToString()
            };
            return uriBuilder.Uri;
        }

        public static Guid GetTeam(this HttpContext httpContext)
        {
            var t = httpContext.User?.FindFirst("clientId")?.Value;
            if (!string.IsNullOrWhiteSpace(t) && t.MatchToGuid())
                return new Guid(t);
            return Guid.Empty;
        }

        public static Guid GetSubId(this HttpContext httpContext)
        {
            var t = httpContext.User?.FindFirst("sub")?.Value;
            if (!string.IsNullOrWhiteSpace(t) && t.MatchToGuid())
                return new Guid(t);
            return Guid.Empty;
        }
        public static string GetSubName(this HttpContext httpContext)
        {
            return httpContext.User?.FindFirst("name")?.Value ?? "";
        }

        public static bool IsRegistered(this HttpContext httpContext)
        {
            var t = httpContext.User?.FindFirst(IIdentityService.ClientKey)?.Value;
            if (!string.IsNullOrWhiteSpace(t) && t.MatchToGuid())
                return true;
            return false;
        }
    }
}
