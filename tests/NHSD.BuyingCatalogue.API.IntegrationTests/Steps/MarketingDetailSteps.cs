using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
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

        [When(@"a GET request is made for solution (.*)")]
        public async Task WhenAGETRequestIsMadeForSolutionSln(string solutionId)
        {
            _response.Result = await Client.GetAsync(string.Format(ByIdSolutionsUrl, solutionId));
        }

        [Then(@"the solution contains MarketingData of (.*)")]
        public async Task ThenTheSolutionContainsMarketingData(string marketingData)
        {
            var content = await _response.ReadBody();
            content.SelectToken("solution.marketingData").ToString().Should().Be(marketingData);
        }

        [Then(@"the solution contains AboutUrl of (.*)")]
        public async Task ThenTheSolutionContainsAboutUrlOfUrlSln(string aboutUrl)
        {
            var content = await _response.ReadBody();
            content.SelectToken("solution.aboutUrl").ToString().Should().Be(aboutUrl);
        }

        private class MarketingDetailTable
        {
            public string Solution { get; set; }

            public string AboutUrl { get; set; }

            public string Features { get; set; }
        }
    }
}
