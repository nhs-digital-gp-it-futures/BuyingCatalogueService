using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace NHSD.BuyingCatalogue.API.Infrastructure.Filters
{
	/// <summary>
	/// Provides a global exception filter.
	/// </summary>
	internal sealed class CustomExceptionFilter : IExceptionFilter
	{
		/// <summary>
		/// Hosting environment details.
		/// </summary>
		public IHostingEnvironment HostingEnvironment { get; }

		/// <summary>
		/// Provide logging for this instance.
		/// </summary>
		public ILogger<CustomExceptionFilter> Logger { get; }

		/// <summary>
		/// Initialises a new instance of the <see cref="CustomExceptionFilter"/> class.
		/// </summary>
		public CustomExceptionFilter(IHostingEnvironment hostingEnvironment, ILogger<CustomExceptionFilter> logger)
		{
			HostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));
			Logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		/// <summary>
		/// Called after an action has thrown an System.Exception.
		/// </summary>
		/// <param name="context">The Microsoft.AspNetCore.Mvc.Filters.ExceptionContext.</param>
		public void OnException(ExceptionContext context)
		{
			Logger.LogError(new EventId(context.Exception.HResult), context.Exception, context.Exception.Message);

			Exception exception = context.Exception;

			object message;

			if (HostingEnvironment.IsDevelopment())
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
			context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
			context.Result = json;
			context.ExceptionHandled = true;
		}
	}
}
