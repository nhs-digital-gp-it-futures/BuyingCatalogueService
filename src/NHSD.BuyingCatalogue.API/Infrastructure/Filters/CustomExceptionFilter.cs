using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NHSD.BuyingCatalogue.Infrastructure.Exceptions;

namespace NHSD.BuyingCatalogue.API.Infrastructure.Filters
{
    /// <summary>
    /// Provides a global exception filter.
    /// </summary>
    internal sealed class CustomExceptionFilter : IExceptionFilter
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly ILogger<CustomExceptionFilter> logger;

        public CustomExceptionFilter(IWebHostEnvironment webHostEnvironment, ILogger<CustomExceptionFilter> logger)
        {
            this.webHostEnvironment = webHostEnvironment ?? throw new ArgumentNullException(nameof(webHostEnvironment));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Called after an action has thrown an System.Exception.
        /// </summary>
        /// <param name="context">The Microsoft.AspNetCore.Mvc.Filters.ExceptionContext.</param>
        public void OnException(ExceptionContext context)
        {
            Exception exception = context.Exception;

            logger.LogError(new EventId(context.Exception.HResult), exception, exception.Message);

            context.HttpContext.Response.ContentType = "application/json";
            context.HttpContext.Response.StatusCode = exception.ToStatusCode();
            context.Result = exception.ToJsonMessage(webHostEnvironment.IsDevelopment());
            context.ExceptionHandled = true;
        }
    }
}
