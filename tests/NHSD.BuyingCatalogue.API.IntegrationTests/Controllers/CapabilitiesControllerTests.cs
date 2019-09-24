using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using Shouldly;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Controllers
{
    [TestFixture]
    public class CapabilitiesControllerTests : IntegrationTestFixtureBase
    {
        private const string ListCapabilitiesUrl = "http://localhost:8080/api/v1/Capabilities";

        [Test]
        public async Task Get_ListCapabilities_ReturnsSuccess()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                HttpResponseMessage response = await httpClient.GetAsync(ListCapabilitiesUrl);
                response.IsSuccessStatusCode.ShouldBeTrue();
            }
        }

        [Test]
        public async Task Get_ListCapabilities_ReturnsData()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                HttpResponseMessage response = await httpClient.GetAsync(ListCapabilitiesUrl);

                string responseBody = await response.Content.ReadAsStringAsync();

                responseBody.ShouldNotBeNullOrWhiteSpace();
            }
        }
    }
}
