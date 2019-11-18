using System;
using NHSD.BuyingCatalogue.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Application.Solutions.Domain
{
    internal class Contact
    {
        public Contact(IMarketingContactResult contact)
        {
            var concatName = $"{contact.FirstName} {contact.LastName}".Trim();
            Name = String.IsNullOrWhiteSpace(concatName) ? null : concatName;
            PhoneNumber = contact.PhoneNumber;
            Email = contact.Email;
            Department = contact.Department;
        }

        /// <summary>
        /// The full name of the contact
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The phone number of the contact
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// The email address of the contact
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// The department of the contact
        /// </summary>
        public string Department { get; set; }
    }
}
