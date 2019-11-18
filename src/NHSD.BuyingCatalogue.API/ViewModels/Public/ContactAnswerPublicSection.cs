using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Contracts.Solutions;

namespace NHSD.BuyingCatalogue.API.ViewModels.Public
{
    public class ContactAnswerPublicSection
    {
        private readonly List<ContactInformationPublicSection> _contacts;

        public ContactAnswerPublicSection(IEnumerable<IContact> contacts)
            => _contacts = contacts.Select(c => new ContactInformationPublicSection(c)).ToList();

        [JsonProperty("contact-1")]
        public ContactInformationPublicSection Contact1
            => _contacts.FirstOrDefault();

        [JsonProperty("contact-2")]
        public ContactInformationPublicSection Contact2
            => _contacts.Skip(1).FirstOrDefault();
    }
}
