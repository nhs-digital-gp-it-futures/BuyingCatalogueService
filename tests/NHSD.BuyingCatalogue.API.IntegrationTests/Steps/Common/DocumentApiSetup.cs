using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using RestEase;
using TechTalk.SpecFlow;
using WireMock.Admin.Mappings;
using WireMock.Client;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Common
{
    internal static class DocumentApiSetup
    {
        private const string WireMockBaseUrl = "http://localhost:9090";

        public static async Task SendModel(MappingModel model, ScenarioContext context, string mappingKey)
        {
            await AddMapping(model).ConfigureAwait(false);

            if (!context.ContainsKey(mappingKey))
                context[mappingKey] = new List<Guid>();

            if (context[mappingKey] is List<Guid> guidList)
                guidList.Add(model.Guid.Value);
        }

        private static async Task AddMapping(MappingModel model)
        {
            model.Guid = Guid.NewGuid();
            model.Priority = 10;
            var api = RestClient.For<IWireMockAdminApi>(new Uri(WireMockBaseUrl));
            var result = await api.PostMappingAsync(model).ConfigureAwait(false);
            result.Status.Should().Be("Mapping added");
        }

        public static async Task ClearMappings(ScenarioContext context, string mappingKey)
        {
            if (context.ContainsKey(mappingKey) && context[mappingKey] is List<Guid> guidList)
            {
                var api = RestClient.For<IWireMockAdminApi>(new Uri(WireMockBaseUrl));
                await Task.WhenAll(guidList.Select(g => api.DeleteMappingAsync(g)).ToArray()).ConfigureAwait(false);
            }
        }
    }
}
