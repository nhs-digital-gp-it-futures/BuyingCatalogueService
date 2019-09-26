using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

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

        public static async Task<HttpResponseMessage> PostAsJsonAsync(string requestUrl, object content)
        {
            return await PostAsync(requestUrl, JsonConvert.SerializeObject(content));
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
