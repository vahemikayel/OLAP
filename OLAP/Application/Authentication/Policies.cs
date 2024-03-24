namespace OLAP.API.Application.Authentication
{
    public static class Policies
    {
        /// <summary>
        /// Admin
        /// </summary>
        public const string Admin = nameof(Admin);
        /// <summary>
        /// Management
        /// </summary>
        public const string Management = nameof(Management);
        /// <summary>
        /// Partner
        /// </summary>
        public const string Partner = nameof(Partner);
        /// <summary>
        /// Partner User
        /// </summary>
        public const string PartnerUser = nameof(PartnerUser);

        internal static string[] PolicyNames { get; private set; } = new string[] {
                                                                                    Admin,
                                                                                    Management,
                                                                                    Partner,
                                                                                    PartnerUser
                                                                                  };
    }
}
