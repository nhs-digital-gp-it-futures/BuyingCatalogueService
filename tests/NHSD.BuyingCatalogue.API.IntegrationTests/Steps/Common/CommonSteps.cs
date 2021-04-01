using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Common
{
    [Binding]
    internal sealed class CommonSteps
    {
        private static readonly Dictionary<string, string> Tokens = new()
        {
            { "learn-more", "sections.learn-more.answers." },
            {
                "native-mobile-third-party",
                "sections.client-application-types.sections.native-mobile.sections.native-mobile-third-party.answers."
            },
            {
                "native-mobile-connection-details",
                "sections.client-application-types.sections.native-mobile.sections.native-mobile-connection-details.answers."
            },
            {
                "native-mobile-operating-systems",
                "sections.client-application-types.sections.native-mobile.sections.native-mobile-operating-systems.answers."
            },
            {
                "native-mobile-first",
                "sections.client-application-types.sections.native-mobile.sections.native-mobile-first.answers."
            },
            {
                "native-mobile-memory-and-storage",
                "sections.client-application-types.sections.native-mobile.sections.native-mobile-memory-and-storage.answers."
            },
            {
                "native-mobile-hardware-requirements",
                "sections.client-application-types.sections.native-mobile.sections.native-mobile-hardware-requirements.answers."
            },
            {
                "native-mobile-additional-information",
                "sections.client-application-types.sections.native-mobile.sections.native-mobile-additional-information.answers."
            },
            {
                "native-desktop-hardware-requirements",
                "sections.client-application-types.sections.native-desktop.sections.native-desktop-hardware-requirements.answers."
            },
            {
                "native-desktop-connection-details",
                "sections.client-application-types.sections.native-desktop.sections.native-desktop-connection-details.answers."
            },
            {
                "native-desktop-operating-systems",
                "sections.client-application-types.sections.native-desktop.sections.native-desktop-operating-systems.answers."
            },
            {
                "native-desktop-third-party",
                "sections.client-application-types.sections.native-desktop.sections.native-desktop-third-party.answers."
            },
            {
                "native-desktop-memory-and-storage",
                "sections.client-application-types.sections.native-desktop.sections.native-desktop-memory-and-storage.answers."
            },
            {
                "native-desktop-additional-information",
                "sections.client-application-types.sections.native-desktop.sections.native-desktop-additional-information.answers."
            },
            {
                "browser-browsers-supported",
                "sections.client-application-types.sections.browser-based.sections.browser-browsers-supported.answers."
            },
            {
                "browser-plug-ins-or-extensions",
                "sections.client-application-types.sections.browser-based.sections.browser-plug-ins-or-extensions.answers."
            },
            {
                "browser-hardware-requirements",
                "sections.client-application-types.sections.browser-based.sections.browser-hardware-requirements.answers."
            },
            {
                "browser-connectivity-and-resolution",
                "sections.client-application-types.sections.browser-based.sections.browser-connectivity-and-resolution.answers."
            },
            {
                "browser-additional-information",
                "sections.client-application-types.sections.browser-based.sections.browser-additional-information.answers."
            },
            {
                "browser-mobile-first",
                "sections.client-application-types.sections.browser-based.sections.browser-mobile-first.answers."
            },
            { "hosting-type-public-cloud", "sections.hosting-type-public-cloud.answers." },
            { "hosting-type-private-cloud", "sections.hosting-type-private-cloud.answers." },
            { "hosting-type-hybrid", "sections.hosting-type-hybrid.answers." },
            { "hosting-type-on-premise", "sections.hosting-type-on-premise.answers." },
            { "solution-description", "sections.solution-description.answers." },

            // ReSharper disable once StringLiteralTypo
            { "roadMap", "sections.roadmap.answers." },
            { "integrations", "sections.integrations.answers." },
            { "implementation-timescales", "sections.implementation-timescales.answers." },
            { "features", "sections.features.answers." },
            { "capabilities", "sections.capabilities.answers." },
            { "contact-details", "sections.contact-details.answers." },
            { "about-supplier", "sections.about-supplier.answers." },
        };

        private readonly Response response;

        public CommonSteps(Response response)
        {
            this.response = response;
        }

        [Then(@"the response contains the following values")]
        public async Task ResponseContainsTableValues(Table table)
        {
            var context = await response.ReadBody();
            foreach (var row in table.CreateSet<SectionFieldValueTable>())
            {
                var responseValues = new List<string>();
                var token = $"{Tokens[row.Section]}{row.Field}";

                var responseToken = context.SelectToken(token);

                Assert.NotNull(responseToken, "token: {0} not found in {1}", token, context);

                if (responseToken is JValue responseValue)
                {
                    responseValues.Add(responseValue.Value?.ToString());
                }
                else
                {
                    responseValues.AddRange(responseToken.Select(s => s.ToString()));
                }

                responseValues.Should().BeEquivalentTo(row.Value);
            }
        }

        [Then(@"the response does not contain the following fields")]
        public async Task ThenTheResponseDoesNotContainTheFollowingFields(Table table)
        {
            var context = await response.ReadBody();
            foreach (var row in table.CreateSet<SectionFieldValueTable>())
            {
                var token = $"{Tokens[row.Section]}{row.Field}";

                var responseToken = context.SelectToken(token);

                responseToken.Should().BeNullOrEmpty();
            }
        }

        [Then(@"the response contains lists with the following counts")]
        public async Task ThenTheResponseContainsListsWithTheFollowingLengths(Table table)
        {
            var context = await response.ReadBody();
            foreach (var row in table.CreateSet<SectionFieldArrayCountTable>())
            {
                var token = $"{Tokens[row.Section]}{row.Field}";

                var responseToken = context.SelectToken(token);

                responseToken.Should().NotBeNull("token: {0} not found in {1}", token, context);

                responseToken.Should().BeOfType<JArray>();
                if (responseToken is JArray responseArray)
                {
                    responseArray.Count.Should().Be(row.Count);
                }
            }
        }

        [Then(@"the string value of element (.*) is (.*)")]
        public async Task ThenTheStringIs(string token, string requirement)
        {
            var content = await response.ReadBody();
            content.Value<string>(token).Should().Be(requirement);
        }

        [Then(@"the boolean value of element (.*) is (.*)")]
        public async Task ThenTheBooleanIs(string token, bool requirement)
        {
            var content = await response.ReadBody();
            content.Value<bool>(token).Should().Be(requirement);
        }

        [Then(@"the (.*) string does not exist")]
        public async Task ThenTheBrowserHardwareRequirementsValueIsNull(string token)
        {
            var content = await response.ReadBody();
            content.Value<string>(token).Should().Be(null);
        }

        [Then(@"the (.*) element contains")]
        public async Task ThenTheSupportedBrowsersElementContains(string token, Table table)
        {
            var content = table.CreateInstance<ElementContainsStringTable>();
            var context = await response.ReadBody();
            context.SelectToken(token)?
                .Select(s => s.ToString())
                .Should().BeEquivalentTo(content.Elements);
        }

        [UsedImplicitly(ImplicitUseTargetFlags.Members)]
        private sealed class ElementContainsStringTable
        {
            public List<string> Elements { get; init; }
        }

        [UsedImplicitly(ImplicitUseTargetFlags.Members)]
        private sealed class SectionFieldValueTable
        {
            public string Section { get; init; }

            public string Field { get; init; }

            public List<string> Value { get; init; }
        }

        [UsedImplicitly(ImplicitUseTargetFlags.Members)]
        private sealed class SectionFieldArrayCountTable
        {
            public string Section { get; init; }

            public string Field { get; init; }

            public int Count { get; init; }
        }
    }
}
