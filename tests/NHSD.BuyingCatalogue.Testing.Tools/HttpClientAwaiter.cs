using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace NHSD.BuyingCatalogue.Testing.Tools
{
    public static class HttpClientAwaiter
    {
        public static async Task<bool> WaitForGetAsync(string url, TimeSpan testTimeout)
        {
            using (var httpClient = new HttpClient() { Timeout = TimeSpan.FromSeconds(1) })
            {
                DateTime startTime = DateTime.UtcNow;
                while (DateTime.UtcNow - startTime < testTimeout)
                {
                    try
                    {
                        var response = await httpClient.GetAsync(new Uri(url)).ConfigureAwait(false);
                        if (response.IsSuccessStatusCode)
                        {
                            return true;
                        }
                    }
                    catch
                    {
                        // Ignore exceptions, just retry
                    }

                    await Task.Delay(1000).ConfigureAwait(false);
                }
            }

            return false;
        }
    }
}
