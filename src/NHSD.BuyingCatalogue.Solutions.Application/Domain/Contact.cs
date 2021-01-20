using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Domain
{
    internal sealed class Contact
    {
        public Contact(IMarketingContactResult contact)
        {
            FirstName = contact.FirstName;
            LastName = contact.LastName;
            PhoneNumber = contact.PhoneNumber;
            Email = contact.Email;
            Department = contact.Department;
        }

        internal Contact(ISupplierResult supplier)
        {
            FirstName = supplier.PrimaryContactFirstName;
            LastName = supplier.PrimaryContactLastName;
            Email = supplier.PrimaryContactEmailAddress;
            PhoneNumber = supplier.PrimaryContactTelephone;
        }

        /// <summary>
        /// Gets or sets the full name of the contact.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the full name of the contact.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets the full name of the contact.
        /// </summary>
        public string Name => GetName();

        /// <summary>
        /// Gets or sets the phone number of the contact.
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the email address of the contact.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the department of the contact.
        /// </summary>
        public string Department { get; set; }

        private string GetName()
        {
            var concatName = $"{FirstName} {LastName}".Trim();
            return string.IsNullOrWhiteSpace(concatName) ? null : concatName;
        }
    }
}
