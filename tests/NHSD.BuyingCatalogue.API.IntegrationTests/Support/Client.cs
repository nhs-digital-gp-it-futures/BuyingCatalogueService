using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Support
{
    public static class Client
    {
        public static async Task<HttpResponseMessage> GetAsync(string requestUrl)
        {
            using HttpClient client = new HttpClient();
            return await client.GetAsync(requestUrl);
        }

        public static async Task<HttpResponseMessage> PostAsJsonAsync(string requestUrl, object content)
        {
            return await PostAsync(requestUrl, JsonConvert.SerializeObject(content));
        }

        public static async Task<HttpResponseMessage> PostAsync(string requestUrl, string content, string contentType = "application/json")
        {
            using HttpClient client = new HttpClient();
            return await client.PostAsync(requestUrl, new StringContent(content, Encoding.UTF8, contentType));
        }

        public static async Task<HttpResponseMessage> PutAsJsonAsync(string requestUrl, JObject content)
        {
            return await PutAsync(requestUrl, content);
        }

        public static async Task<HttpResponseMessage> PutAsync(string requestUrl, JObject content, string contentType = "application/json")
        {
            using HttpClient client = new HttpClient();
            return await client.PutAsync(requestUrl, new StringContent(content.ToString(), Encoding.UTF8, contentType));
        }

        public static async Task<HttpResponseMessage> PutAsJsonAsync(string requestUrl, object content)
        {
            return await PutAsync(requestUrl, JsonConvert.SerializeObject(content));
        }

        public static async Task<HttpResponseMessage> PutAsync(string requestUrl, object content, string contentType = "application/json")
        {
            using HttpClient client = new HttpClient();
            return await client.PutAsync(requestUrl, new StringContent(content.ToString(), Encoding.UTF8, contentType));
        }

        public static async Task<HttpResponseMessage> PutAsync(string requestUrl)
        {
            using HttpClient client = new HttpClient();
            return await client.PutAsync(requestUrl, null);
        }
    }
}
