using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Infrastructure;
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
            DepartmentName = contact?.Department.NullIfWhitespace();
            ContactName = contact?.Name.NullIfWhitespace();
            PhoneNumber = contact?.PhoneNumber.NullIfWhitespace();
            EmailAddress = contact?.Email.NullIfWhitespace();
        }

        public bool IsPopulated()
            => DepartmentName != null
               || ContactName != null
               || PhoneNumber != null
               || EmailAddress != null;

        public ContactInformationSection IfPopulated()
            => IsPopulated() ? this : null;
    }
}
