using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels
{
    public sealed class GetContactDetailsResult
    {
        private readonly List<GetContactDetailsResultSection> _contacts;

        public GetContactDetailsResult(IEnumerable<IContact> contacts)
            => _contacts = contacts?.Select(c => new GetContactDetailsResultSection(c)).ToList();

        [JsonProperty("contact-1")]
        public GetContactDetailsResultSection Contact1
            => _contacts?.FirstOrDefault();

        [JsonProperty("contact-2")]
        public GetContactDetailsResultSection Contact2
            => _contacts?.Skip(1).FirstOrDefault();
    }

    public sealed class GetContactDetailsResultSection
    {
        internal GetContactDetailsResultSection (IContact contact)
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
