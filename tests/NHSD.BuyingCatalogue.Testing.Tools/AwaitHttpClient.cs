﻿using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace NHSD.BuyingCatalogue.Testing.Tools
{
    public static class AwaitHttpClient
    {
        public static async Task<bool> WaitForGetAsync(string address, TimeSpan testTimeout)
        {
            using var httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(1) };

            DateTime startTime = DateTime.UtcNow;
            while (DateTime.UtcNow - startTime < testTimeout)
            {
                try
                {
                    using var response = await httpClient.GetAsync(new Uri(address));
                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                }
                catch (Exception e) when (e is HttpRequestException || e is TaskCanceledException)
                {
                    // Ignore exceptions, just retry
                }

                await Task.Delay(1000);
            }

            return false;
        }
    }
}
