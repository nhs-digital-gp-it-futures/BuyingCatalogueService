using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using JetBrains.Annotations;
using NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Common;
using NHSD.BuyingCatalogue.API.IntegrationTests.Support;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Testing.Data.EntityBuilders;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.AdditionalService
{
    [Binding]
    internal sealed class AdditionalServiceSteps
    {
        private const string AdditionalServiceUrl = "http://localhost:5200/api/v1/additional-services";

        private readonly Response response;

        public AdditionalServiceSteps(Response response)
        {
            this.response = response;
        }

        [Given(@"AdditionalService exist")]
        public static async Task GivenAdditionalServiceExist(Table table)
        {
            foreach (var additionalService in table.CreateSet<AdditionalServiceTable>())
            {
                await CatalogueItemEntityBuilder.Create()
                    .WithCatalogueItemId(additionalService.CatalogueItemId)
                    .WithCatalogueItemTypeId((int)CatalogueItemType.AdditionalService)
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
            var solutionIds = table.CreateSet<SolutionIdTable>().Select(t => $"solutionIds={t.SolutionId}").ToList();

            if (solutionIds.Count is 0)
            {
                response.Result = await Client.GetAsync(AdditionalServiceUrl);
            }
            else
            {
                var query = string.Join("&", solutionIds);

                response.Result = await Client.GetAsync($"{AdditionalServiceUrl}?{query}");
            }
        }

        [Then(@"Additional Services are returned")]
        public async Task ThenAdditionalServicesAreReturned(Table table)
        {
            var expected = table.CreateSet<ExpectedAdditionalServiceTable>().ToList();

            const string solutionToken = "solution";

            var responseBody = await response.ReadBody();
            var additionalServicesToken = responseBody.SelectToken("additionalServices");

            var actual = additionalServicesToken?.Select(t => new ExpectedAdditionalServiceTable
            {
                CatalogueItemId = t.Value<string>("catalogueItemId"),
                Name = t.Value<string>("name"),
                Summary = t.Value<string>("summary"),
                SolutionId = t.SelectToken(solutionToken)?.Value<string>("solutionId"),
                SolutionName = t.SelectToken(solutionToken)?.Value<string>("name"),
            });

            actual.Should().BeEquivalentTo(expected);
        }

        [Then(@"no additional services are returned")]
        public async Task ThenNoAdditionalServicesAreReturned()
        {
            var responseBody = await response.ReadBody();
            var additionalServicesToken = responseBody.SelectToken("additionalServices");

            additionalServicesToken.Should().BeEmpty();
        }

        [UsedImplicitly(ImplicitUseTargetFlags.Members)]
        private sealed class AdditionalServiceTable
        {
            public string CatalogueItemId { get; init; }

            public string CatalogueItemName { get; init; }

            public string CatalogueSupplierId { get; init; }

            public string Summary { get; init; }

            public string SolutionId { get; init; }
        }

        [UsedImplicitly(ImplicitUseTargetFlags.Members)]
        private sealed class SolutionIdTable
        {
            public string SolutionId { get; init; }
        }

        [UsedImplicitly(ImplicitUseTargetFlags.Members)]
        private sealed class ExpectedAdditionalServiceTable
        {
            public string CatalogueItemId { get; init; }

            public string Name { get; init; }

            public string Summary { get; init; }

            public string SolutionId { get; init; }

            public string SolutionName { get; init; }
        }
    }
}
