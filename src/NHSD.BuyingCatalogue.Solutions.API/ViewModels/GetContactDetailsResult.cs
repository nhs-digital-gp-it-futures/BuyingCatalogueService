using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels
{
    public sealed class GetContactDetailsResult
    {
        private readonly List<GetContactDetailsResultSection> contacts;

        public GetContactDetailsResult(IEnumerable<IContact> contacts) =>
            this.contacts = contacts?.Select(c => new GetContactDetailsResultSection(c)).ToList();

        [JsonProperty("contact-1")]
        public GetContactDetailsResultSection Contact1 => contacts?.FirstOrDefault();

        [JsonProperty("contact-2")]
        public GetContactDetailsResultSection Contact2 => contacts?.Skip(1).FirstOrDefault();
    }
}
