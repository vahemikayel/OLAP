namespace OLAP.API.Services
{
    public interface IIdentityService
    {
        internal static string ClientKey = "clientId";

        string GetUserIdentity();
        Guid GetIdentity();
        string GetScope();
        int GetUserType();
        Guid GetTeam();
        Guid GetSubId();
        Uri GetAbsoluteUri();
        bool IsRegistered();
        string[] GetUserRoles();
    }
}
