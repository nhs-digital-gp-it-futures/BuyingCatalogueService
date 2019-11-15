using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.API.ViewModels.Public
{
    public class ContactAnswerPublicSection
    {
        public ContactAnswerPublicSection()
        {
            Contact1 = new ContactInformationPublicSection();
            Contact2 = new ContactInformationPublicSection();
        }
        [JsonProperty("contact-1")]
        public ContactInformationPublicSection Contact1 { get; }

        [JsonProperty("contact-2")]
        public ContactInformationPublicSection Contact2 { get; }
    }
}
