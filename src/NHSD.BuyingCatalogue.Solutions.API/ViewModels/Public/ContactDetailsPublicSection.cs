using System.Collections.Generic;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Public
{
    public class ContactDetailsPublicSection
    {
        public ContactDetailsPublicSection(IEnumerable<IContact> contacts)
            => Answers = new ContactAnswerPublicSection(contacts);

        [JsonProperty("answers")]
        public ContactAnswerPublicSection Answers { get; }
    }
}
