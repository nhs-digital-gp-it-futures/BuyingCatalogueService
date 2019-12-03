using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NHSD.BuyingCatalogue.Infrastructure.Exceptions
{
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Converts the specified exception into a HTTP status code. By default return <see cref="StatusCodes.Status500InternalServerError"/>.
        /// </summary>
        /// <param name="exception">Error details.</param>
        /// <returns>The HTTP status code determined by the specified <paramref name="exception"/>.</returns>
        public static int ToStatusCode(this Exception exception)
        {
            int statusCode = StatusCodes.Status500InternalServerError;

            if (exception is NotFoundException)
            {
                statusCode = StatusCodes.Status404NotFound;
            }

            return statusCode;
        }

        public static JsonResult ToJsonMessage(this Exception exception, bool verbose)
        {
            verbose = true;

            return new JsonResult(new
            {
                errors = new[] { "An unexpected error occurred." },
                detail = verbose ? exception.ToString() : exception.Message
            });
        }
    }
}
