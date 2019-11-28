using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionPlugins
{
    public sealed class UpdateSolutionContactDetailsViewModel
    {
        [JsonProperty("contact-1")]
        public UpdateSolutionContactViewModel Contact1 { get; set; }

        [JsonProperty("contact-2")]
        public UpdateSolutionContactViewModel Contact2 { get; set; }
    }

    public sealed class UpdateSolutionContactViewModel
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
