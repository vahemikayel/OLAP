namespace OLAP.API.Application.Authentication
{
    /// <summary>
    /// System Roles
    /// </summary>
    internal class Roles
    {
        /// <summary>
        /// Administrator
        /// </summary>
        internal const string Admin = nameof(Admin);

        /// <summary>
        /// Add, Edit, Delete Partner
        /// </summary>
        internal const string ManagePartner = nameof(ManagePartner);

        /// <summary>
        /// Activate/Disactivate Partner
        /// </summary>
        internal const string ActivatePartner = nameof(ActivatePartner);

        /// <summary>
        /// Organisation, who can manage its users
        /// </summary>
        internal const string Partner = nameof(Partner);

        /// <summary>
        /// User of Organisation
        /// </summary>
        internal const string PartnerUser = nameof(PartnerUser);

        internal static string[] RoleNames { get; private set; } = new string[] {
                                                                                    Admin,
                                                                                    ManagePartner,
                                                                                    ActivatePartner,
                                                                                    Partner,
                                                                                    PartnerUser
                                                                               };
    }
}
