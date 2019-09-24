using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Shouldly;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Drivers
{
    public class HttpClientDriver
    {
        private HttpClient _httpClient;
        private HttpResponseMessage _httpResponseMessage;

        public void CreateHttpClient()
        {
            _httpClient = new HttpClient();
        }

        public async Task GetAsync(string url)
        {
            _httpResponseMessage = await _httpClient.GetAsync(url);
        }

        public async Task<string> ReadContentAsync()
        {
            return await _httpResponseMessage.Content.ReadAsStringAsync();
        }

        public void CheckResponseStatus(HttpStatusCode expectedHttpStatusCode)
        {
            _httpResponseMessage.StatusCode.ShouldBe(expectedHttpStatusCode);
        }
    }
}
