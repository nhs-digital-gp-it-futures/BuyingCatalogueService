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

        protected abstract string InsertSql { get; }

        public DateTime LastUpdated { get; set; }

        public Guid LastUpdatedBy { get; set; }

        public async Task InsertAsync()
            => await SqlRunner.ExecuteAsync(ConnectionStrings.GPitFuturesSetup, InsertSql, this).ConfigureAwait(false);
    }
}
