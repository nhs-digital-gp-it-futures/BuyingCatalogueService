using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts.Commands;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels
{
    public sealed class UpdateSolutionContactViewModel : IUpdateSolutionContact
    {
        [JsonProperty("department-name")]
        public string Department { get; set; }

        [JsonProperty("email-address")]
        public string Email { get; set; }

        [JsonProperty("first-name")]
        public string FirstName { get; set; }

        [JsonProperty("last-name")]
        public string LastName { get; set; }

        [JsonProperty("phone-number")]
        public string PhoneNumber { get; set; }

        public bool HasData()
        {
            return !(string.IsNullOrWhiteSpace(FirstName) &&
                     string.IsNullOrWhiteSpace(LastName) &&
                     string.IsNullOrWhiteSpace(PhoneNumber) &&
                     string.IsNullOrWhiteSpace(Email) &&
                     string.IsNullOrWhiteSpace(Department));
        }

        public void Trim()
        {
            Department = Department?.Trim();
            FirstName = FirstName?.Trim();
            LastName = LastName?.Trim();
            PhoneNumber = PhoneNumber?.Trim();
            Email = Email?.Trim();
        }
    }
}
