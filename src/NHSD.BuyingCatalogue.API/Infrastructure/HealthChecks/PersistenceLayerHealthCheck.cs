using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using NHSD.BuyingCatalogue.API.Properties;
using NHSD.BuyingCatalogue.Contracts.Infrastructure.HealthChecks;
using NHSD.BuyingCatalogue.Infrastructure;

namespace NHSD.BuyingCatalogue.API.Infrastructure.HealthChecks
{
    public class PersistenceLayerHealthCheck : IHealthCheck
    {
        private readonly IRepositoryHealthCheck _repositoryHealthCheck;
        private readonly ILogger<PersistenceLayerHealthCheck> _logger;

        /// <summary>
        /// Initialises a new instance of the <see cref="PersistenceLayerHealthCheck"/> class.
        /// </summary>
        public PersistenceLayerHealthCheck(IRepositoryHealthCheck repositoryHealthCheck, ILogger<PersistenceLayerHealthCheck> logger)
        {
            _repositoryHealthCheck = repositoryHealthCheck ?? throw new ArgumentNullException(nameof(repositoryHealthCheck));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Runs the health check, returning the status of the component being checked.
        /// </summary>
        /// <param name="context">A context object associated with the current execution.</param>
        /// <param name="cancellationToken">A <see cref="System.Threading.CancellationToken"/> that can be used to cancel the health check.</param>
        /// <returns>A <see cref="Task{HealthCheckResult}"/> that completes when the health check has finished, yielding the status of the component being checked.</returns>
        [SuppressMessage("Design", "CA1031", Justification = "We want to catch all exceptions as this is the health check purpose")]
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            HealthCheckResult healthStatus = HealthCheckResult.Healthy();

            try
            {
                await _repositoryHealthCheck.RunAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, Resources.HealthCheckFailed);

                healthStatus = new HealthCheckResult(context.ThrowIfNull().Registration.FailureStatus, exception: exception);
            }

            return healthStatus;
        }
    }
}
