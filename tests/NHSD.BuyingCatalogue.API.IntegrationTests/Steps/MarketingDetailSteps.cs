using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NHSD.BuyingCatalogue.API.IntegrationTests.Support;
using NHSD.BuyingCatalogue.Testing.Data.Entities;
using NHSD.BuyingCatalogue.Testing.Data.EntityBuilders;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps
{
    [Binding]
    internal sealed class MarketingDetailSteps
    {
        private const string ByIdSolutionsUrl = "http://localhost:8080/api/v1/Solutions/{0}";

        private const string FeaturesUrl = "http://localhost:8080/api/v1/Solutions/{0}/sections/features";

        private readonly ScenarioContext _context;

        private readonly Response _response;

        public MarketingDetailSteps(ScenarioContext context, Response response)
        {
            _context = context;
            _response = response;
        }

        [Given(@"MarketingDetail exist")]
        public async Task GivenMarketingDetailExist(Table table)
        {
            foreach (var marketingDetail in table.CreateSet<MarketingDetailTable>())
            {
                await MarketingDetailEntityBuilder.Create()
                    .WithFeatures(marketingDetail.Features)
                    .WithAboutUrl(marketingDetail.AboutUrl)
                    .WithSolutionId(marketingDetail.Solution)
                    .Build()
                    .InsertAsync();
            }
        }

        [Given(@"a MarketingDetail (.*) does not exist")]
        public async Task GivenAMarketingDetailDoesNotExist(string solutionId)
        {
            var marketingDetailList = await MarketingDetailEntity.FetchAllAsync();
            marketingDetailList.Select(x => x.SolutionId).Should().NotContain(solutionId);
        }

        [When(@"a GET request is made for solution (.*)")]
        public async Task WhenAGETRequestIsMadeForSolutionSln(string solutionId)
        {
            _response.Result = await Client.GetAsync(string.Format(ByIdSolutionsUrl, solutionId));
        }

        [When(@"a PUT request is made to update solution (.*) features section")]
        public async Task GivenAPUTRequestIsMadeToUpdateSolutionSlnFeatures(string solutionId, Table table)
        {
            var content = table.CreateInstance<FeaturesPostTable>();

            _response.Result = await Client.PutAsJsonAsync(string.Format(FeaturesUrl, solutionId), new { Listing = content.Features });
        }

        [Then(@"the solution contains AboutUrl of (.*)")]
        public async Task ThenTheSolutionContainsAboutUrlOfUrlSln(string aboutUrl)
        {
            var content = await _response.ReadBody();
            content.SelectToken("solution.marketingData.sections[?(@.id == 'solution-description')].data.link").ToString().Should().Be(aboutUrl);
        }

        [Then(@"the solution does not contain AboutUrl")]
        public async Task ThenTheSolutionDoesNotContainAboutUrl()
        {
            var content = await _response.ReadBody();
            content.SelectToken("solution.marketingData.sections[?(@.id == 'solution-description')].data.link").Should().BeNull();
        }

        [Then(@"the solution contains Features")]
        public async Task ThenTheSolutionContainsFeatures(Table table)
        {
            var content = await _response.ReadBody();
            content.SelectToken("solution.marketingData.sections[?(@.id == 'features')].data.listing")
                .Select(s => s.ToString()).Should().BeEquivalentTo(table.CreateSet<FeaturesTable>().Select(s => s.Feature));
        }

        [Then(@"the solution contains no features")]
        public async Task ThenTheSolutionContainsNoFeatures()
        {
            var content = await _response.ReadBody();
            content.SelectToken("solution.marketingData.sections[?(@.id == 'features')].data.listing")
                .Should().BeNullOrEmpty();
        }

        [Then(@"MarketingDetail exist")]
        public async Task ThenMarketingDetailExist(Table table)
        {
            var expectedMarketingDetails = table.CreateSet<MarketingDetailTable>().Select(m => new
            {
                m.Solution,
                AboutUrl = string.IsNullOrWhiteSpace(m.AboutUrl) ? null : m.AboutUrl,
                Features = string.IsNullOrWhiteSpace(m.Features) ? null : m.Features
            });
            var marketingDetails = await MarketingDetailEntity.FetchAllAsync();
            marketingDetails.Select(m => new
            {
                Solution = m.SolutionId,
                m.AboutUrl,
                m.Features
            }).Should().BeEquivalentTo(expectedMarketingDetails);
        }

        private class MarketingDetailTable
        {
            public string Solution { get; set; }

            public string AboutUrl { get; set; }

            public string Features { get; set; }
        }

        private class MarketingDetailPostTable
        {
            public string Summary { get; set; }

            public string Description { get; set; }

            public string AboutUrl { get; set; }

            public string MarketingData { get; set; }
        }

        private class FeaturesPostTable
        {
            public List<string> Features { get; set; }
        }

        private class FeaturesTable
        {
            public string Feature { get; set; }
        }
    }
}
