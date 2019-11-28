using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NHSD.BuyingCatalogue.Data.Infrastructure
{
    public interface IDbConnector
    {
        Task<IEnumerable<T>> QueryAsync<T>(CancellationToken cancellationToken, string sql, object args = null);

        Task ExecuteAsync(CancellationToken cancellationToken, string sql, object args = null);

        Task ExecuteMultipleWithTransactionAsync(CancellationToken cancellationToken, IEnumerable<(string sql, object args)> functions);
    }
}
