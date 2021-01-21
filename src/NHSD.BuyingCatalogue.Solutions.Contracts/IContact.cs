namespace NHSD.BuyingCatalogue.Solutions.Contracts
{
    public interface IContact
    {
        /// <summary>
        /// Gets the first name of the contact, as displayed to the user.
        /// </summary>
        string FirstName { get; }

        /// <summary>
        /// Gets the last name of the contact, as displayed to the user.
        /// </summary>
        string LastName { get; }

        /// <summary>
        /// Gets the full name of the contact, as displayed to the user.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the phone number of the contact, as displayed to the user.
        /// </summary>
        string PhoneNumber { get; }

        /// <summary>
        /// Gets the email address of the contact, as displayed to the user.
        /// </summary>
        string Email { get; }

        /// <summary>
        /// Gets the department of the contact, as displayed to the user.
        /// </summary>
        string Department { get; }
    }
}
