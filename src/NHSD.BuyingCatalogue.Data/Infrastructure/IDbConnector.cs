using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NHSD.BuyingCatalogue.Data.Infrastructure
{
    public interface IDbConnector
    {
        Task<T> QueryFirstOrDefaultAsync<T>(string sql, CancellationToken cancellationToken, object param = null);

        Task<IEnumerable<T>> QueryAsync<T>(string sql, CancellationToken cancellationToken, object args = null);

        Task ExecuteAsync(string sql, CancellationToken cancellationToken, object args = null);

        Task ExecuteMultipleWithTransactionAsync(
            IEnumerable<(string Sql, object Args)> functions,
            CancellationToken cancellationToken);
    }
}
