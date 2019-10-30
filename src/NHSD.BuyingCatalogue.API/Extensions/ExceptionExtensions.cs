using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Application.Exceptions;

namespace NHSD.BuyingCatalogue.API.Extensions
{
    internal static class ExceptionExtensions
    {
        /// <summary>
        /// Converts the specified exception into a HTTP status code. By default return <see cref="StatusCodes.Status500InternalServerError"/>.
        /// </summary>
        /// <param name="exception">Error details.</param>
        /// <returns>The HTTP status code determined by the specified <paramref name="exception"/>.</returns>
        internal static int ToStatusCode(this Exception exception)
        {
            int statusCode = StatusCodes.Status500InternalServerError;

            if (exception is NotFoundException)
            {
                statusCode = StatusCodes.Status404NotFound;
            }

            return statusCode;
        }

        internal static JsonResult ToJsonMessage(this Exception exception, bool verbose)
        {
            return new JsonResult(new
            {
                errors = new[] { "An unexpected error occurred." },
                detail = verbose ? exception.ToString() : exception.Message
            });
        }
    }
}
