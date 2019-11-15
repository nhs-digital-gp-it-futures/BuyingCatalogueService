using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.API.ViewModels.Public
{
    public class ContactDetailsPublicSection
    {
        public ContactDetailsPublicSection()
        {
            Answers = new ContactAnswerPublicSection();
        }

        [JsonProperty("answers")]
        public ContactAnswerPublicSection Answers { get; }
    }
}
