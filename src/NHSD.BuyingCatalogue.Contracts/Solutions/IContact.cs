using System;
using System.Collections.Generic;
using System.Text;

namespace NHSD.BuyingCatalogue.Contracts.Solutions
{
    public interface IContact
    {
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
