using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Contracts.Solutions;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Public
{
    public class ContactInformationPublicSection
    {
        [JsonProperty("department-name")]
        public string DepartmentName { get; }

        [JsonProperty("contact-name")]
        public string ContactName { get; }

        [JsonProperty("phone-number")]
        public string PhoneNumber { get; }

        [JsonProperty("email-address")]
        public string EmailAddress { get; }

        public ContactInformationPublicSection(IContact contact)
        {
            DepartmentName = contact.Department;
            ContactName = contact.Name;
            PhoneNumber = contact.PhoneNumber;
            EmailAddress = contact.Email;
        }
    }
}
