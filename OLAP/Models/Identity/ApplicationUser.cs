using Microsoft.AspNetCore.Identity;

namespace OLAP.API.Models.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PartnerId { get; set; }
    }
}
