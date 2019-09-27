using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NHSD.BuyingCatalogue.Application.Exceptions;

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
            _logger.LogError(new EventId(context.Exception.HResult), context.Exception, context.Exception.Message);

			Exception exception = context.Exception;

            object message;

			if (_webhostEnvironment.IsDevelopment())
			{
				message = new
				{
					errors = new[] { "An unexpected error occurred." },
					detail = exception.ToString()
				};
			}
			else
			{
				message = new
				{
					errors = new[] { "An unexpected error occurred." },
					detail = exception.Message
				};
			}

			JsonResult json = new JsonResult(message);

			context.HttpContext.Response.ContentType = "application/json";
            context.HttpContext.Response.StatusCode = ConvertToStatusCode(exception);
			context.Result = json;
			context.ExceptionHandled = true;
		}

        /// <summary>
        /// Converts the specified exception into a HTTP status code. By default return <see cref="StatusCodes.Status500InternalServerError"/>.
        /// </summary>
        /// <param name="exception">Error details.</param>
        /// <returns>The HTTP status code determined by the specified <paramref name="exception"/>.</returns>
        private int ConvertToStatusCode(Exception exception)
        {
            int statusCode = StatusCodes.Status500InternalServerError;

            if (exception is NotFoundException)
            {
                statusCode = StatusCodes.Status404NotFound;
            }

            return statusCode;
        }
    }
}
