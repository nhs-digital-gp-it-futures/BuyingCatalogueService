using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution
{
    public class ContactInformationSection
    {
        [JsonProperty("department-name")]
        public string DepartmentName { get; }

        [JsonProperty("contact-name")]
        public string ContactName { get; }

        [JsonProperty("phone-number")]
        public string PhoneNumber { get; }

        [JsonProperty("email-address")]
        public string EmailAddress { get; }

        public ContactInformationSection(IContact contact)
        {
            DepartmentName = contact.Department;
            ContactName = contact.Name;
            PhoneNumber = contact.PhoneNumber;
            EmailAddress = contact.Email;
        }
    }
}
