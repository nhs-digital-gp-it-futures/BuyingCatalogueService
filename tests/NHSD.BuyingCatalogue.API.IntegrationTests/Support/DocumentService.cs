﻿using System;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Testing.Tools;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Support
{
    internal static class DocumentService
    {
        private const string WaitServerUrl = "http://localhost:5201/";

        private static readonly TimeSpan TestTimeout = TimeSpan.FromSeconds(60);

        internal static async Task AwaitApiRunningAsync()
        {
            await AwaitApiRunningAsync(WaitServerUrl);
        }

        internal static async Task AwaitApiRunningAsync(string url)
        {
            var started = await HttpClientAwaiter.WaitForGetAsync(url, TestTimeout);
            if (!started)
            {
                throw new TimeoutException(
                    $"Start Document API failed, could not get a successful health status from '{url}' after trying for '{TestTimeout}'");
            }
        }
    }
}
