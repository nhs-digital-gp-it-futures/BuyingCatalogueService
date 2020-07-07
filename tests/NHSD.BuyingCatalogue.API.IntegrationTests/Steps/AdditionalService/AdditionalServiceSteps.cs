using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Common;
using NHSD.BuyingCatalogue.API.IntegrationTests.Support;
using NHSD.BuyingCatalogue.Testing.Data.EntityBuilders;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.AdditionalService
{
    [Binding]
    internal sealed class AdditionalServiceSteps
    {
        private readonly Response _response;

        private const string additionalServiceUrl = "http://localhost:5200/api/v1/additional-services";

        public AdditionalServiceSteps(Response response)
        {
            _response = response;
        }

        [Given(@"AdditionalService exist")]
        public static async Task GivenAdditionalServiceExist(Table table)
        {
            foreach (var additionalService in table.CreateSet<AdditionalServiceTable>())
            {
                await CatalogueItemEntityBuilder.Create()
                    .WithCatalogueItemId(additionalService.CatalogueItemId)
                    .WithName(additionalService.CatalogueItemName)
                    .WithSupplierId(additionalService.CatalogueSupplierId)
                    .Build()
                    .InsertAsync();

                await AdditionalServiceEntityBuilder.Create()
                    .WithCatalogueItemId(additionalService.CatalogueItemId)
                    .WithSummary(additionalService.Summary)
                    .WithSolutionId(additionalService.SolutionId)
                    .Build()
                    .InsertAsync();
            }
        }

        [When(@"a Get request is made to retrieve the additional services with solutionIds")]
        public async Task WhenAGetRequestIsMadeToRetrieveTheAdditionalServicesWithSolutionIds(Table table)
        {
            var solutionIds = table.CreateSet<SolutionIdTable>().Select(x => $"solutionIds={x.SolutionId}").ToList();

            if (solutionIds.Count is 0)
            {
                _response.Result = await Client.GetAsync(additionalServiceUrl);
            }
            else
            {
                var query = string.Join("&", solutionIds);

                _response.Result = await Client.GetAsync($"{additionalServiceUrl}?{query}");
            }
        }

        [Then(@"Additional Services are returned")]
        public async Task ThenAdditionalServicesAreReturned(Table table)
        {
            var expected = table.CreateSet<ExpectedAdditionalServiceTable>().ToList();

            const string solutionToken = "solution";

            var content = (await _response.ReadBody()).Select(x => new ExpectedAdditionalServiceTable
            {
                AdditionalServiceId = x.Value<string>("additionalServiceId"),
                Name = x.Value<string>("name"),
                Summary = x.Value<string>("summary"),
                SolutionId = x.SelectToken(solutionToken).Value<string>("solutionId"),
                SolutionName = x.SelectToken(solutionToken).Value<string>("name")
            });

            content.Should().BeEquivalentTo(expected);
        }

        private sealed class AdditionalServiceTable
        {
            public string CatalogueItemId { get; set; }
            public string CatalogueItemName { get; set; }
            public string CatalogueSupplierId { get; set; }
            public string Summary { get; set; }
            public string SolutionId { get; set; }
        }

        private sealed class SolutionIdTable
        {
            public string SolutionId { get; set; }
        }

        private sealed class ExpectedAdditionalServiceTable
        {
            public string AdditionalServiceId { get; set; }
            public string Name { get; set; }
            public string Summary { get; set; }
            public string SolutionId { get; set; }
            public string SolutionName { get; set; }
        }
    }
}
