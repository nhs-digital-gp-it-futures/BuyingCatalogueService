using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Application.Solutions.Domain;

namespace NHSD.BuyingCatalogue.API.ViewModels
{
    public sealed class BrowserBasedResult
    {
        public BrowserBasedResult (IClientApplication clientApplication)
        {
            Sections = new List<BrowserBasedResultSection>
            {
                new BrowserBasedResultSection("browsers-supported", BrowserSupportedComplete(clientApplication), true),
                new BrowserBasedResultSection("plug-ins-or-extensions", false, true),
                new BrowserBasedResultSection("connectivity-and-resolution", false, true),
                new BrowserBasedResultSection("hardware-requirements", false, false ),
                new BrowserBasedResultSection("additional-information", false, false )
            };
        }

        private bool BrowserSupportedComplete(IClientApplication clientApplication)
        {
            return clientApplication?.BrowsersSupported?.Any() == true && clientApplication?.MobileResponsive.HasValue == true;
        }

        [JsonProperty("sections")]
        public List<BrowserBasedResultSection> Sections { get; }
    }

    public class BrowserBasedResultSection
    {
        private readonly bool _mandatory;
        private readonly bool _complete;

        public BrowserBasedResultSection(string id, bool complete, bool mandatory)
        {
            Id = id;
            _complete = complete;
            _mandatory = mandatory;
        }

        [JsonProperty("id")]
        public string Id { get;  }

        [JsonProperty("status")]
        public string Status => _complete ? "COMPLETE" : "INCOMPLETE";

        [JsonProperty("requirement")]
        public string Requirement => _mandatory ? "Mandatory" : "Optional";
    }
}
