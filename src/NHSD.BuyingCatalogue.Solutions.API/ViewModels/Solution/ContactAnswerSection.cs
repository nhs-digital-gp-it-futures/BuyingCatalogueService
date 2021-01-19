using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution
{
    public sealed class ContactAnswerSection
    {
        private readonly List<ContactInformationSection> contacts;

        public ContactAnswerSection(IEnumerable<IContact> contacts)
            => this.contacts = contacts.Select(c => new ContactInformationSection(c)).ToList();

        [JsonProperty("contact-1")]
        public ContactInformationSection Contact1
            => contacts.FirstOrDefault()?.IfPopulated();

        [JsonProperty("contact-2")]
        public ContactInformationSection Contact2
            => contacts.Skip(1).FirstOrDefault()?.IfPopulated();

        public bool HasData()
            => Contact1?.IsPopulated() == true || Contact2?.IsPopulated() == true;
    }
}
