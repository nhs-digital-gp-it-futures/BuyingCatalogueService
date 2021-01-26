using System.Collections.Generic;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution
{
    public sealed class ContactDetailsSection
    {
        public ContactDetailsSection(IEnumerable<IContact> contacts) => Answers = new ContactAnswerSection(contacts);

        [JsonProperty("answers")]
        public ContactAnswerSection Answers { get; }

        public ContactDetailsSection IfPopulated() => Answers.HasData() ? this : null;
    }
}
