using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NHSD.BuyingCatalogue.Data.Infrastructure
{
    public interface IDbConnector
    {
        Task<IEnumerable<T>> QueryAsync<T>(string sql,CancellationToken cancellationToken, object args = null);

        Task ExecuteAsync(string sql, CancellationToken cancellationToken, object args = null);

        Task ExecuteMultipleWithTransactionAsync(IEnumerable<(string sql, object args)> functions, CancellationToken cancellationToken);
    }
}
