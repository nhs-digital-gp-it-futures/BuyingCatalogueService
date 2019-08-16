using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace NHSD.BuyingCatalogue.Persistence.Infrastructure
{
	/// <summary>
	/// Defines the data contract representing a factory to provide a new connection to the data store.
	/// </summary>
	public interface IDbConnectionFactory
	{
		/// <summary>
		/// Gets a new database connection.
		/// </summary>
		/// <returns>A task representing an operation to create a new database connection.</returns>
		Task<IDbConnection> GetAsync(CancellationToken cancellationToken);
	}
}
