using System;
using Microsoft.AspNetCore.Http;
using Serilog.Events;

namespace NHSD.BuyingCatalogue.API.Infrastructure.Logging
{
    public static class SerilogRequestLoggingOptions
    {
        public const string HealthCheckEndpointDisplayName = "Health checks";

        public static LogEventLevel GetLevel(HttpContext httpContext,double value, Exception exception)
        {
            if (exception != null)
                return LogEventLevel.Error;

            if (httpContext == null || httpContext.Response.StatusCode > 499)
                return LogEventLevel.Error;

            return IsHealthCheck(httpContext)
                ? LogEventLevel.Verbose
                : LogEventLevel.Information;
        }

        private static bool IsHealthCheck(HttpContext httpContext)
        {
            var endpoint = httpContext.GetEndpoint();

            return endpoint != null && string.Equals(
                endpoint.DisplayName,
                HealthCheckEndpointDisplayName,
                StringComparison.OrdinalIgnoreCase);
        }
    }
}
