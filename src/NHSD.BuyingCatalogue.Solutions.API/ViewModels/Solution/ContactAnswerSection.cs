using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution
{
    public class ContactAnswerSection
    {
        private readonly List<ContactInformationSection> _contacts;

        public ContactAnswerSection(IEnumerable<IContact> contacts)
            => _contacts = contacts.Select(c => new ContactInformationSection(c)).ToList();

        [JsonProperty("contact-1")]
        public ContactInformationSection Contact1
            => _contacts.FirstOrDefault();

        [JsonProperty("contact-2")]
        public ContactInformationSection Contact2
            => _contacts.Skip(1).FirstOrDefault();
    }
}
