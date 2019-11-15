using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.API.ViewModels.Public
{
    public class ContactDetailsPublicSection
    {
        [JsonProperty("answers")]
        public ContactAnswerPublicSection Answers { get; }
    }
}
