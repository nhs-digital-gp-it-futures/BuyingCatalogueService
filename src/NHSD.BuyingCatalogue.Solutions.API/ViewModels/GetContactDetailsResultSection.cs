using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels
{
    public sealed class GetContactDetailsResultSection
    {
        internal GetContactDetailsResultSection(IContact contact)
        {
            DepartmentName = contact.Department;
            EmailAddress = contact.Email;
            FirstName = contact.FirstName;
            LastName = contact.LastName;
            PhoneNumber = contact.PhoneNumber;
        }

        [JsonProperty("department-name")]
        public string DepartmentName { get; }

        [JsonProperty("first-name")]
        public string FirstName { get; }

        [JsonProperty("last-name")]
        public string LastName { get; }

        [JsonProperty("phone-number")]
        public string PhoneNumber { get; }

        [JsonProperty("email-address")]
        public string EmailAddress { get; }
    }
}
