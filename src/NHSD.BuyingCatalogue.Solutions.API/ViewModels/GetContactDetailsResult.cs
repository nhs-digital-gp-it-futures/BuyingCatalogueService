using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels
{
    public sealed class GetContactDetailsResult
    {
        [JsonProperty("contact-1")]
        public GetContactDetailsResultSection Contact1 { get; set; }

        [JsonProperty("contact-2")]
        public GetContactDetailsResultSection Contact2 { get; set; }
    }

    public sealed class GetContactDetailsResultSection
    {
        [JsonProperty("department-name")]
        public string DepartmentName { get; set; }

        [JsonProperty("first-name")]
        public string FirstName { get; set; }

        [JsonProperty("last-name")]
        public string LastName { get; set; }

        [JsonProperty("phone-number")]
        public string PhoneNumber { get; set; }

        [JsonProperty("email-address")]
        public string EmailAddress { get; set; }
    }
}
