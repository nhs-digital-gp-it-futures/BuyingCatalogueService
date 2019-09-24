using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Support
{
    public static class Client
    {
        public static async Task<HttpResponseMessage> GetAsync(string requestUrl)
        {
            HttpResponseMessage response = null;

            using (HttpClient client = new HttpClient())
            {
                response = await client.GetAsync(requestUrl);
            }

            return response;
        }

        public static async Task<HttpResponseMessage> PostAsync(string requestUrl, string content, string contentType = "application/json")
        {
            HttpResponseMessage response = null;

            using (HttpClient client = new HttpClient())
            {
                response = await client.PostAsync(requestUrl, new StringContent(content, Encoding.UTF8, contentType));
            }

            return response;
        }
    }
}
