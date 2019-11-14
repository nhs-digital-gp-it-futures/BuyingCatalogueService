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

        protected string NullOrWrapQuotes(string candidate)
        {
            return candidate == null ? "NULL" : $"'{candidate}'";
        }

        protected int ToOneZero(bool value)
        {
            return value ? 1 : 0;
        }
    }
}
