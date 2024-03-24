namespace OLAP.API.Models.Response
{
    /// <summary>
    /// User
    /// </summary>
    public class ApplicationUserResponseModel
    {
        /// <summary>
        /// Gets or sets the primary key for this user.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the user name for this user.
        /// </summary>
        public string UserName { get; set; }

        ////
        //// Summary:
        ////     Gets or sets the normalized user name for this user.
        //public string NormalizedUserName { get; set; }

        /// <summary>
        /// Gets or sets the email address for this user.
        /// </summary>
        public string Email { get; set; }

        ////
        //// Summary:
        ////     Gets or sets the normalized email address for this user.
        //public string NormalizedEmail { get; set; }

        ////
        //// Summary:
        ////     Gets or sets a flag indicating if a user has confirmed their email address.
        ////
        //// Value:
        ////     True if the email address has been confirmed, otherwise false.
        //public bool EmailConfirmed { get; set; }

        /// <summary>
        /// Gets or sets the PhoneNumber for this user.
        /// </summary>
        public string PhoneNumber { get; set; }

    }
}
