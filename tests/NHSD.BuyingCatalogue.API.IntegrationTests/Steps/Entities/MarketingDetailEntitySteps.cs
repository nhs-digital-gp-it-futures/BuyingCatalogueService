using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using NHSD.BuyingCatalogue.Testing.Data.Entities;
using NHSD.BuyingCatalogue.Testing.Data.EntityBuilders;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Entities
{
    [Binding]
    public sealed class MarketingDetailEntitySteps
    {

        [Given(@"MarketingDetail exist")]
        public async Task GivenMarketingDetailExist(Table table)
        {
            foreach (var marketingDetail in table.CreateSet<MarketingDetailTable>())
            {
                await SolutionDetailEntityBuilder.Create()
                    .WithFeatures(marketingDetail.Features)
                    .WithSummary(marketingDetail.SummaryDescription == "NULL" ? null : marketingDetail.SummaryDescription )
                    .WithFullDescription(marketingDetail.FullDescription)
                    .WithAboutUrl(marketingDetail.AboutUrl)
                    .WithSolutionId(marketingDetail.Solution)
                    .WithClientApplication(marketingDetail.ClientApplication)
                    .Build()
                    .InsertAndSetCurrentForSolutionAsync();
            }
        }

        [Given(@"a MarketingDetail (.*) does not exist")]
        public async Task GivenAMarketingDetailDoesNotExist(string solutionId)
        {
            var marketingDetailList = await SolutionDetailEntity.FetchAllAsync();
            marketingDetailList.Select(x => x.SolutionId).Should().NotContain(solutionId);
        }

        [Then(@"MarketingDetail exist")]
        public async Task ThenMarketingDetailExist(Table table)
        {
            var expectedMarketingDetails = table.CreateSet<MarketingDetailTable>().Select(m => new
            {
                m.Solution,
                AboutUrl = string.IsNullOrWhiteSpace(m.AboutUrl) ? null : m.AboutUrl,
                Features = string.IsNullOrWhiteSpace(m.Features) ? null : m.Features,
                ClientApplication = string.IsNullOrWhiteSpace(m.ClientApplication) ? null : JToken.Parse(m.ClientApplication).ToString()
            });
            var marketingDetails = await SolutionDetailEntity.FetchAllAsync();
            marketingDetails.Select(m => new
            {
                Solution = m.SolutionId,
                m.AboutUrl,
                m.Features,
                ClientApplication = string.IsNullOrWhiteSpace(m.ClientApplication) ? null : JToken.Parse(m.ClientApplication).ToString()
            }).Should().BeEquivalentTo(expectedMarketingDetails);
        }

        private class MarketingDetailTable
        {
            public string Solution { get; set; }

            public string SummaryDescription { get; set; }

            public string FullDescription { get; set; }

            public string AboutUrl { get; set; }

            public string Features { get; set; }

            public string ClientApplication { get; set; }
        }
    }
}
