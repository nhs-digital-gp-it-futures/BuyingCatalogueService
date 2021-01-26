using System;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Support
{
    internal static class Client
    {
        internal static async Task<HttpResponseMessage> GetAsync(string requestUrl)
        {
            using HttpClient client = new HttpClient();
            return await client.GetAsync(new Uri(requestUrl));
        }

        internal static async Task<HttpResponseMessage> PostAsJsonAsync(string requestUrl, object content)
        {
            return await PostAsync(requestUrl, JsonConvert.SerializeObject(content));
        }

        internal static async Task<HttpResponseMessage> PostAsync(
            string requestUrl,
            string content,
            string contentType = MediaTypeNames.Application.Json)
        {
            using HttpClient client = new HttpClient();
            using var stringContent = new StringContent(content, Encoding.UTF8, contentType);
            return await client.PostAsync(new Uri(requestUrl), stringContent);
        }

        internal static async Task<HttpResponseMessage> PutAsJsonAsync(string requestUrl, object content)
        {
            return await PutAsync(requestUrl, JsonConvert.SerializeObject(content));
        }

        internal static async Task<HttpResponseMessage> PutAsync(string requestUrl)
        {
            using HttpClient client = new HttpClient();
            return await client.PutAsync(new Uri(requestUrl), null!);
        }

        private static async Task<HttpResponseMessage> PutAsync(
            string requestUrl,
            object content,
            string contentType = MediaTypeNames.Application.Json)
        {
            using HttpClient client = new HttpClient();
            using var stringContent = new StringContent(content.ToString()!, Encoding.UTF8, contentType);
            return await client.PutAsync(new Uri(requestUrl), stringContent);
        }
    }
}
