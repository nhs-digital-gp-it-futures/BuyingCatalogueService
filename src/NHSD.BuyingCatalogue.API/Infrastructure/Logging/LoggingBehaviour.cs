using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace NHSD.BuyingCatalogue.API.Infrastructure.Logging
{
    public sealed class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<LoggingBehaviour<TRequest, TResponse>> logger;

        public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest, TResponse>> logger)
        {
            this.logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (next is null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            try
            {
                var stopWatch = new Stopwatch();
                stopWatch.Start();

                var response = await next();

                stopWatch.Stop();
                logger.LogInformation(
                    "Handled {requestType} in {elapsedMilliseconds}ms with request: {@request} and response: {@response} ",
                    typeof(TRequest).Name,
                    stopWatch.ElapsedMilliseconds,
                    request,
                    response);

                return response;
            }
            catch (Exception e)
            {
                logger.LogError(e, "ERROR {@request}", request);
                throw;
            }
        }
    }
}
