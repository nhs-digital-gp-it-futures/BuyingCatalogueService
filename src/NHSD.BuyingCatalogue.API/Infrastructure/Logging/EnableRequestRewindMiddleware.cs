using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using NHSD.BuyingCatalogue.Infrastructure;

namespace NHSD.BuyingCatalogue.API.Infrastructure.Logging
{
    public sealed class EnableRequestRewindMiddleware
    {
        private readonly RequestDelegate _next;

        public EnableRequestRewindMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        // Do not remove! This method is used in the extension
        public async Task Invoke(HttpContext context)
        {
            context.ThrowIfNull().Request.EnableBuffering();
            await _next(context).ConfigureAwait(false);
        }
    }

    public static class EnableRequestRewindExtension
    {
        public static IApplicationBuilder UseEnableRequestRewind(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<EnableRequestRewindMiddleware>();
        }
    }
}
