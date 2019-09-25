using System.Threading.Tasks;

namespace NHSD.BuyingCatalogue.Testing.Data.Entities
{
    public abstract class EntityBase
    {
        protected abstract string InsertSql { get; }

        public async Task InsertAsync()
        {
            await SqlRunner.ExecuteAsync(ConnectionStrings.GPitFuturesSetup, InsertSql);
        }
    }
}
