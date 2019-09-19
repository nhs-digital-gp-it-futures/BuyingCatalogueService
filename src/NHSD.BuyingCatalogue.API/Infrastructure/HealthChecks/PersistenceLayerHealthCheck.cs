using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using NHSD.BuyingCatalogue.Application.Infrastructure.HealthChecks;

namespace NHSD.BuyingCatalogue.API.Infrastructure.HealthChecks
{
    public class PersistenceLayerHealthCheck : IHealthCheck
    {
        private readonly IRepositoryHealthCheck _repositoryHealthCheck;

        /// <summary>
        /// Initialises a new instance of the <see cref="PersistenceLayerHealthCheck"/> class.
        /// </summary>
        public PersistenceLayerHealthCheck(IRepositoryHealthCheck repositoryHealthCheck)
        {
            _repositoryHealthCheck = repositoryHealthCheck ?? throw new ArgumentNullException(nameof(repositoryHealthCheck));
        }

        /// <summary>
        /// Runs the health check, returning the status of the component being checked.
        /// </summary>
        /// <param name="context">A context object associated with the current execution.</param>
        /// <param name="cancellationToken">A <see cref="System.Threading.CancellationToken"/> that can be used to cancel the health check.</param>
        /// <returns>A <see cref="Task{HealthCheckResult}"/> that completes when the health check has finished, yielding the status of the component being checked.</returns>
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            HealthCheckResult healthStatus = HealthCheckResult.Healthy();

            try
            {
                await _repositoryHealthCheck.RunAsync(cancellationToken);
            }
            catch (Exception exception)
            {
                healthStatus = new HealthCheckResult(context.Registration.FailureStatus, exception: exception);
            }

            return healthStatus;
        }
    }
}
