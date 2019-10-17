using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NHSD.BuyingCatalogue.Testing.Data.Entities
{
    public class SolutionSupplierStatusEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public static async Task<IEnumerable<SolutionSupplierStatusEntity>> FetchAllAsync()
        {
            return await SqlRunner.FetchAllAsync<SolutionSupplierStatusEntity>($@"
            SELECT  [Id]
                    ,[Name]
            FROM    SolutionSupplierStatus");
        }

        public static async Task<SolutionSupplierStatusEntity> GetByNameAsync(string name, StringComparison stringComparison = StringComparison.OrdinalIgnoreCase)
        {
            return (await FetchAllAsync()).First(item => name.Equals(item.Name, stringComparison));
        }
    }
}
