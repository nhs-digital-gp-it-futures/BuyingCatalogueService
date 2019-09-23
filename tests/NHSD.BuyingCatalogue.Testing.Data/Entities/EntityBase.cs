using System.Threading.Tasks;

namespace NHSD.BuyingCatalogue.Testing.Data.Entities
{
    public abstract class EntityBase
    {
        protected abstract string InsertSql { get; }

        public async Task Insert()
        {
            await SqlRunner.ExecuteAsync(Database.ConnectionStringSetup, InsertSql);
        }
    }
}
