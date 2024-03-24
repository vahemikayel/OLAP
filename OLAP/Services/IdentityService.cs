using OLAP.API.Extensions;

namespace OLAP.API.Services
{
    public class IdentityService : IIdentityService
    {
        private IHttpContextAccessor _context;

        public IdentityService(IHttpContextAccessor context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Guid GetIdentity()
        {
            return _context.HttpContext.GetIdentity();
        }
        public string GetScope()
        {

            return _context.HttpContext.User.Claims.Where(x => x.Type == "scope")
                                                   .Select(x => x.Value)
                                                   .FirstOrDefault();
        }

        public string GetUserIdentity()
        {
            return _context.HttpContext.GetUserIdentity();
        }

        public int GetUserType()
        {
            return _context.HttpContext.GetUserType();
        }

        public Uri GetAbsoluteUri()
        {
            return _context.HttpContext.GetAbsoluteUri();
        }

        public Guid GetTeam()
        {
            return _context.HttpContext.GetTeam();
        }

        //public Guid GetSalePortal()
        //{
        //    return _context.HttpContext.GetSalePortal();
        //}
        public bool IsRegistered()
        {
            return _context.HttpContext.IsRegistered();
        }

        public string[] GetUserRoles()
        {
            return _context.HttpContext.GetUserRoles();

        }

        public Guid GetSubId()
        {
            return _context.HttpContext.GetSubId();
        }
    }
}
