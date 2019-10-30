using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NHSD.BuyingCatalogue.API.Extensions;

namespace NHSD.BuyingCatalogue.API.Infrastructure.Filters
{
    /// <summary>
    /// Provides a global exception filter.
    /// </summary>
    internal sealed class CustomExceptionFilter : IExceptionFilter
	{
        private readonly IWebHostEnvironment _webhostEnvironment;
        private readonly ILogger<CustomExceptionFilter> _logger;

		/// <summary>
		/// Initialises a new instance of the <see cref="CustomExceptionFilter"/> class.
		/// </summary>
		public CustomExceptionFilter(IWebHostEnvironment webhostEnvironment, ILogger<CustomExceptionFilter> logger)
		{
            _webhostEnvironment = webhostEnvironment ?? throw new ArgumentNullException(nameof(webhostEnvironment));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		/// <summary>
		/// Called after an action has thrown an System.Exception.
		/// </summary>
		/// <param name="context">The Microsoft.AspNetCore.Mvc.Filters.ExceptionContext.</param>
		public void OnException(ExceptionContext context)
		{
            Exception exception = context.Exception;

            _logger.LogError(new EventId(context.Exception.HResult), exception, exception.Message);

            context.HttpContext.Response.ContentType = "application/json";
            context.HttpContext.Response.StatusCode = exception.ToStatusCode();
			context.Result = exception.ToJsonMessage(_webhostEnvironment.IsDevelopment());
			context.ExceptionHandled = true;
		}
    }
}
