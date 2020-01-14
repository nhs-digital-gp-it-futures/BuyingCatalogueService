using System.Linq;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Testing.Data.Entities;
using NHSD.BuyingCatalogue.Testing.Data.EntityBuilders;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Entities
{
    [Binding]
    public sealed class SupplierEntitySteps
    {
        [Given(@"Suppliers exist")]
        public static async Task GivenSuppliersExist(Table table)
        {
            foreach (var supplier in table.CreateSet<SupplierTable>())
            {
                await InsertSupplierAsync(supplier).ConfigureAwait(false);
            }
        }

        private static async Task InsertSupplierAsync(SupplierTable supplierTable)
        {
            var organisations = (await OrganisationEntity.FetchAllAsync().ConfigureAwait(false)).ToList();

            await SupplierEntityBuilder.Create()
                .WithId(supplierTable.Id)
                .WithOrganisation(organisations.First(o => o.Name == supplierTable.OrganisationName).Id)
                .WithName(supplierTable.SupplierName)
                .Build()
                .InsertAsync()
                .ConfigureAwait(false);
        }

        private class SupplierTable
        {
            public string Id { get; set; }

            public string SupplierName { get; set; }

            public string OrganisationName { get; set; }
        }
    }
}
