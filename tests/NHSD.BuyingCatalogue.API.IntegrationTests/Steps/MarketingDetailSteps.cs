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

        [Given(@"a POST request is made to update solution (.*) features")]
        public async Task GivenAPOSTRequestIsMadeToUpdateSolutionSlnFeatures(string solutionId, Table table)
        {
            var baseContent =  table.CreateInstance<MarketingDetailPostTable>();

            var content = JsonConvert.SerializeObject(new { baseContent.Summary, baseContent.AboutUrl, baseContent.Description, MarketingData = 22222}).ToString();
            content = content.Replace("22222", baseContent.MarketingData);

            _response.Result = await Client.PutAsJsonAsync(string.Format(ByIdSolutionsUrl, solutionId), JObject.Parse(content));
        }

        [Then(@"the solution contains MarketingData of (.*)")]
        public async Task ThenTheSolutionContainsMarketingData(string marketingData)
        {
            var content = await _response.ReadBody();
            content.SelectToken("solution.marketingData").ToString().Should().Be(marketingData);
        }

        [Then(@"the solution contains MarketingData")]
        public async Task ThenTheSolutionContainsMarketingData(Table table)
        {
            var content = await _response.ReadBody();
            foreach (var marketingDetail in table.CreateSet<MarketingDataTable>())
            {
                content.SelectToken(marketingDetail.JsonPath).ToString().Should().Be(marketingDetail.Value);
            }
        }

        [Then(@"the solution contains AboutUrl of (.*)")]
        public async Task ThenTheSolutionContainsAboutUrlOfUrlSln(string aboutUrl)
        {
            var content = await _response.ReadBody();
            content.SelectToken("solution.aboutUrl").ToString().Should().Be(aboutUrl);
        }

        [Then(@"MarketingDetail exist")]
        public async Task ThenMarketingDetailExist(Table table)
        {
            var expectedMarketingDetails = table.CreateSet<MarketingDetailTable>().Select(m => new
            {
                m.Solution,
                m.AboutUrl,
                Features = JObject.Parse(m.Features).ToString()
            });
            var marketingDetails = await MarketingDetailEntity.FetchAllAsync();
            marketingDetails.Select(m => new
            {
                Solution = m.SolutionId,
                m.AboutUrl,
                Features = JObject.Parse(m.Features).ToString()
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

        private class MarketingDataTable
        {
            public string JsonPath { get; set; }

            public string Value { get; set; }
        }
    }
}
