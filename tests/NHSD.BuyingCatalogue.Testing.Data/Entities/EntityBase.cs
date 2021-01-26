using System;
using System.Threading.Tasks;

namespace NHSD.BuyingCatalogue.Testing.Data.Entities
{
    public abstract class EntityBase
    {
        protected EntityBase()
        {
            LastUpdated = DateTime.UtcNow;
            LastUpdatedBy = Guid.Empty;
        }

        public DateTime LastUpdated { get; set; }

        public Guid LastUpdatedBy { get; set; }

        protected abstract string InsertSql { get; }

        public async Task InsertAsync() => await SqlRunner.ExecuteAsync(ConnectionStrings.GPitFuturesSetup, InsertSql, this);

        public async Task<T> InsertAsync<T>() => await SqlRunner.QueryFirstAsync<T>(InsertSql, this);
    }
}
