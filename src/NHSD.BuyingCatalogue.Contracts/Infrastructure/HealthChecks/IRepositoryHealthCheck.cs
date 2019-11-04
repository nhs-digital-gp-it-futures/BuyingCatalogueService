using System.Threading;
using System.Threading.Tasks;

namespace NHSD.BuyingCatalogue.Contracts.Infrastructure.HealthChecks
{
    /// <summary>
    /// Defines a contract representing a health check to verify the status of the data access layer.
    /// </summary>
    public interface IRepositoryHealthCheck
    {
        /// <summary>
        /// Activates the status check of the repository.
        /// </summary>
        /// <param name="cancellationToken">A token to cancel the action.</param>
        /// <returns>A task representing an operation to check the health of the repository.</returns>
        Task RunAsync(CancellationToken cancellationToken);
    }
}
