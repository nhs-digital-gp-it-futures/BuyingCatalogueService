using System;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Http;
using NHSD.BuyingCatalogue.Infrastructure;
using Serilog;
using Serilog.Events;

namespace NHSD.BuyingCatalogue.API.Infrastructure.Logging
{
    public static class LogHelper
    {
        public static void EnrichFromRequest(IDiagnosticContext diagnosticContext, HttpContext httpContext)
        {
            diagnosticContext = diagnosticContext.ThrowIfNull();

            httpContext = httpContext.ThrowIfNull();

            var request = httpContext.Request;
            
            // Set all the common properties available for every request
            diagnosticContext.Set("Host", request.Host);
            diagnosticContext.Set("Protocol", request.Protocol);
            diagnosticContext.Set("Scheme", request.Scheme);

            // Only set it if available. You're not sending sensitive data in a querystring right?!
            if (request.QueryString.HasValue)
            {
                diagnosticContext.Set("QueryString", request.QueryString.Value);
            }

            request.EnableBuffering();
            using (var reader = new StreamReader(
                request.Body,
                encoding: Encoding.UTF8,
                detectEncodingFromByteOrderMarks: false,
                //bufferSize: bufferSize,
                leaveOpen: true))
            {
                
                // Reset the request body stream position
                request.Body.Position = 0;
                var task = reader.ReadToEndAsync();
                task.Wait();
                var body = task.Result;

                // Do some processing with body…
                if (!string.IsNullOrWhiteSpace(body))
                {
                    diagnosticContext.Set("Body", body);
                }

                // Reset the request body stream position so the next middleware can read it
                request.Body.Position = 0;
            }


            // Set the content-type of the Response at this point
            diagnosticContext.Set("ContentType", httpContext.Response.ContentType);

            // Retrieve the IEndpointFeature selected for the request
            var endpoint = httpContext.GetEndpoint();
            if (endpoint != null)
            {
                diagnosticContext.Set("EndpointName", endpoint.DisplayName);
            }
        }

        private static bool IsHealthCheckEndpoint(HttpContext ctx)
        {
            var endpoint = ctx.GetEndpoint();
            if (endpoint != null) 
            {
                return string.Equals(
                    endpoint.DisplayName,
                    "Health checks",
                    StringComparison.Ordinal);
            }

            // No endpoint, so not a health check endpoint
            return false;
        }

        public static LogEventLevel ExcludeHealthChecks(HttpContext ctx, double elapsedMs, Exception ex) =>
            ex != null
                ? LogEventLevel.Error
                : ctx == null || ctx.Response.StatusCode > 499
                    ? LogEventLevel.Error
                    : IsHealthCheckEndpoint(ctx) // Not an error, check if it was a health check
                        ? LogEventLevel.Verbose // Was a health check, use Verbose
                        : LogEventLevel.Information;
    }
}

