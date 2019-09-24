using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.API.IntegrationTests.Support;
using NHSD.BuyingCatalogue.Testing.Data.EntityBuilders;
using NUnit.Framework;
using Shouldly;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Controllers
{
    [TestFixture]
    public class SolutionsControllerTests : IntegrationTestFixtureBase
    {
        private const string ListSolutionsUrl = "http://localhost:8080/api/v1/Solutions";

        [Test]
        public async Task Get_ListSolutions_ReturnsSuccess()
        {
            var response = await Client.GetAsync(ListSolutionsUrl);

            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Test]
        public async Task Get_ListSolutions_ReturnsData()
        {
            var expectedSolution = SolutionEntityBuilder.Create().WithName("Sol1").Build();
            await expectedSolution.Insert();

            var expectedCapability = CapabilityEntityBuilder.Create().WithName("Cap1").Build();
            await expectedCapability.Insert();

            var expectedSolutionCapabilityEntity = SolutionCapabilityEntityBuilder.Create()
                .WithSolutionId(expectedSolution.Id)
                .WithCapabilityId(expectedCapability.Id)
                .Build();

            await expectedSolutionCapabilityEntity.Insert();

            var response = await Client.GetAsync(ListSolutionsUrl);
            var responseContent = await response.Content.ReadAsStringAsync();

            responseContent.ShouldNotBeNullOrWhiteSpace();
        }

        [Test]
        public async Task Post_ListSolutions_ReturnsSuccess()
        {
            var response = await Client.PostAsync(ListSolutionsUrl, "{ }");

            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Test]
        public async Task Post_ListSolutions_ReturnsData()
        {
            var response = await Client.PostAsync(ListSolutionsUrl, "{ }");
            string responseBody = await response.Content.ReadAsStringAsync();

            responseBody.ShouldNotBeNullOrWhiteSpace();
        }

        [Test]
        public async Task Post_ListSolutionsWithCapabilityFilter_ReturnsSuccess()
        {
            var response = await Client.PostAsync(ListSolutionsUrl, BuildCapabilityFilter());

            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Test]
        public async Task Post_ListSolutionsWithCapabilityFilter_ReturnsData()
        {
            var response = await Client.PostAsync(ListSolutionsUrl, BuildCapabilityFilter());
            string responseBody = await response.Content.ReadAsStringAsync();

            responseBody.ShouldNotBeNullOrWhiteSpace();
        }

        private string BuildCapabilityFilter()
        {
            var data = new
            {
                Capabilities = new HashSet<Guid>()
                {
                    Guid.Parse("E4D22F8A-0F5F-43D5-A8E4-60F1365E968A")
                }
            };

            return JsonConvert.SerializeObject(data);
        }
    }
}
