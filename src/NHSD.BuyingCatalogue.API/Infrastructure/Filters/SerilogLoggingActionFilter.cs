using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace NHSD.BuyingCatalogue.API.Infrastructure.Filters
{
    public sealed class SerilogLoggingActionFilter : IActionFilter
    {
        private readonly IDiagnosticContext diagnosticContext;

        public SerilogLoggingActionFilter(IDiagnosticContext diagnosticContext)
        {
            this.diagnosticContext = diagnosticContext;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            diagnosticContext.Set("RouteData", context.ActionDescriptor.RouteValues);
            diagnosticContext.Set("ActionName", context.ActionDescriptor.DisplayName);
            diagnosticContext.Set("ActionId", context.ActionDescriptor.Id);
            diagnosticContext.Set("ValidationState", context.ModelState.IsValid);
        }

        // Required by the interface
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
