namespace NHSD.BuyingCatalogue.Solutions.Contracts
{
    public interface IContact
    {
        /// <summary>
        /// The first name of the contact, as displayed to the user
        /// </summary>
        string FirstName { get; }

        /// <summary>
        /// The last name of the contact, as displayed to the user
        /// </summary>
        string LastName { get; }

        /// <summary>
        /// The full name of the contact, as displayed to the user.
        /// </summary>
        string Name { get; }
        /// <summary>
        /// The phone number of the contact, as displayed to the user.
        /// </summary>
        string PhoneNumber { get; }
        /// <summary>
        /// The email address of the contact, as displayed to the user.
        /// </summary>
        string Email { get; }
        /// <summary>
        /// The department of the contact, as displayed to the user.
        /// </summary>
        string Department { get; }
    }
}
