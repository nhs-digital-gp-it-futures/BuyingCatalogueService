using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Common
{
    [Binding]
    internal sealed class CommonSteps
    {
        private static readonly Dictionary<string, string> Tokens = new Dictionary<string, string>
        {
            { "native-mobile-third-party", "sections.client-application-types.sections.native-mobile.sections.native-mobile-third-party.answers." },
            { "native-mobile-connection-details", "sections.client-application-types.sections.native-mobile.sections.native-mobile-connection-details.answers." },
            { "native-mobile-operating-systems", "sections.client-application-types.sections.native-mobile.sections.native-mobile-operating-systems.answers." },
            { "native-mobile-first", "sections.client-application-types.sections.native-mobile.sections.native-mobile-first.answers." },
            { "native-mobile-memory-and-storage", "sections.client-application-types.sections.native-mobile.sections.native-mobile-memory-and-storage.answers." },
            { "native-mobile-hardware-requirements", "sections.client-application-types.sections.native-mobile.sections.native-mobile-hardware-requirements.answers." },
            { "native-mobile-additional-information", "sections.client-application-types.sections.native-mobile.sections.native-mobile-additional-information.answers." },
            { "native-desktop-hardware-requirements", "sections.client-application-types.sections.native-desktop.sections.native-desktop-hardware-requirements.answers." },
            { "native-desktop-connection-details", "sections.client-application-types.sections.native-desktop.sections.native-desktop-connection-details.answers." },
            { "native-desktop-operating-systems", "sections.client-application-types.sections.native-desktop.sections.native-desktop-operating-systems.answers." },
            { "native-desktop-third-party", "sections.client-application-types.sections.native-desktop.sections.native-desktop-third-party.answers." },
            { "native-desktop-memory-and-storage", "sections.client-application-types.sections.native-desktop.sections.native-desktop-memory-and-storage.answers." },
            { "native-desktop-additional-information", "sections.client-application-types.sections.native-desktop.sections.native-desktop-additional-information.answers." },
            { "browser-browsers-supported","sections.client-application-types.sections.browser-based.sections.browser-browsers-supported.answers." },
            { "browser-plug-ins-or-extensions","sections.client-application-types.sections.browser-based.sections.browser-plug-ins-or-extensions.answers." },
            { "browser-hardware-requirements","sections.client-application-types.sections.browser-based.sections.browser-hardware-requirements.answers." },
            { "browser-connectivity-and-resolution","sections.client-application-types.sections.browser-based.sections.browser-connectivity-and-resolution.answers." },
            { "browser-additional-information","sections.client-application-types.sections.browser-based.sections.browser-additional-information.answers." },
            { "browser-mobile-first","sections.client-application-types.sections.browser-based.sections.browser-mobile-first.answers." },
            { "hosting-type-public-cloud","sections.hosting-type-public-cloud.answers." },
            { "hosting-type-private-cloud","sections.hosting-type-private-cloud.answers." },
            { "hosting-type-hybrid","sections.hosting-type-hybrid.answers." },
            { "hosting-type-on-premise","sections.hosting-type-on-premise.answers." },
            { "solution-description","sections.solution-description.answers." },
            { "roadmap","sections.roadmap.answers." },
            { "integrations","sections.integrations.answers." },
            { "implementation-timescales","sections.implementation-timescales.answers." },
            { "features","sections.features.answers." },
            { "capabilities", "sections.capabilities.answers."},
            { "contact-details", "sections.contact-details.answers." },
            { "about-supplier", "sections.about-supplier.answers."}
        };

        private readonly Response _response;

        public CommonSteps(Response response)
        {
            _response = response;
        }

        [Then(@"the response contains the following values")]
        public async Task ResponseContainsTableValues(Table table)
        {
            foreach (var row in table.CreateSet<SectionFieldValueTable>())
            {
                var responseValues = new List<string>();
                var context = await _response.ReadBody().ConfigureAwait(false);

                var token = $"{Tokens[row.Section]}{row.Field}";
                var responseToken = context.SelectToken(token);

                if (responseToken is JValue responseValue)
                {
                    responseValues.Add(responseValue.Value.ToString());
                }
                else
                {
                    responseValues.AddRange(responseToken.Select(s => s.ToString()));
                }

                responseValues.Should().BeEquivalentTo(row.Value);
            }
        }

        [Then(@"the string value of element (.*) is (.*)")]
        public async Task ThenTheStringIs(string token, string requirement)
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.Value<string>(token).Should().Be(requirement);
        }

        [Then(@"the (.*) string does not exist")]
        public async Task ThenTheBrowserHardwareRequirementsValueIsNull(string token)
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.Value<string>(token).Should().Be(null);
        }

        [Then(@"the (.*) element contains")]
        public async Task ThenTheSupportedBrowsersElementContains(string token, Table table)
        {
            var content = table.CreateInstance<ElementContainsStringTable>();
            var context = await _response.ReadBody().ConfigureAwait(false);
            context.SelectToken(token)
                .Select(s => s.ToString())
                .Should().BeEquivalentTo(content.Elements);
        }

        private class ElementContainsStringTable
        {
            public List<string> Elements { get; set; }
        }

        private class SectionFieldValueTable
        {
            public string Section { get; set; }
            public string Field { get; set; }
            public List<string> Value { get; set; }
        }
    }
}
