using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace NHSD.BuyingCatalogue.Testing.Tools
{
    public static class HttpClientAwaiter
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "We cannot know which exceptions a filed HttpRequest will throw")]
        public static async Task<bool> WaitForGetAsync(string address, TimeSpan testTimeout)
        {
            using (var httpClient = new HttpClient() { Timeout = TimeSpan.FromSeconds(1) })
            {
                DateTime startTime = DateTime.UtcNow;
                while (DateTime.UtcNow - startTime < testTimeout)
                {
                    try
                    {
                        using var response = await httpClient.GetAsync(new Uri(address)).ConfigureAwait(false);
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
