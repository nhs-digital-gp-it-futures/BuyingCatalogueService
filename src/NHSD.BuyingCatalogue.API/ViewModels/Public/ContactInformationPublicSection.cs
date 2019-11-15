using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.API.ViewModels.Public
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

        // Canned Data --Todo
        public ContactInformationPublicSection()
        {
            DepartmentName = "a contact dept";
            ContactName = "jim jones";
            PhoneNumber = "0222 222222";
            EmailAddress = "jacky@solution.com";
        }
    }
}
