using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NHSD.BuyingCatalogue.Testing.Data;
using NHSD.BuyingCatalogue.Testing.Data.Entities;
using NHSD.BuyingCatalogue.Testing.Data.EntityBuilders;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Entities
{
    [Binding]
    internal sealed class SupplierEntitySteps
    {
        [Given(@"Suppliers exist")]
        public static async Task GivenSuppliersExist(Table table)
        {
            foreach (var supplier in table.CreateSet<SupplierTable>())
            {
                await InsertSupplierAsync(supplier).ConfigureAwait(false);
            }
        }

        [Then(@"Suppliers exist")]
        public static async Task ThenSuppliersExist(Table table)
        {
            var expectedSuppliers = table.CreateSet<SupplierTable>().Select(s => new
            {
                s.Id,
                Summary = string.IsNullOrWhiteSpace(s.Summary) ? null : s.Summary,
                SupplierUrl = string.IsNullOrWhiteSpace(s.SupplierUrl) ? null : s.SupplierUrl,
            });

            var suppliers = await SupplierEntity.FetchAllAsync().ConfigureAwait(false);
            suppliers.Select(s => new
            {
                s.Id,
                s.Summary,
                s.SupplierUrl,
            }).Should().BeEquivalentTo(expectedSuppliers);
        }

        [Then(@"Last Updated has updated on the Supplier for supplier (.*)")]
        public static async Task LastUpdatedHasUpdatedOnSupplier(string supplierId)
        {
            var supplier = await SupplierEntity.GetByIdAsync(supplierId).ConfigureAwait(false);
            (await supplier.LastUpdated.SecondsFromNow().ConfigureAwait(false)).Should().BeLessOrEqualTo(5);
        }

        [Given(@"a Supplier (.*) does not exist")]
        public static async Task GivenASolutionSlnDoesNotExist(string supplierId)
        {
            var supplierList = await SupplierEntity.FetchAllAsync();
            supplierList.Select(x => x.Id).Should().NotContain(supplierId);
        }

        private static async Task InsertSupplierAsync(SupplierTable supplierTable)
        {
            await SupplierEntityBuilder.Create()
                .WithId(supplierTable.Id)
                .WithName(supplierTable.SupplierName)
                .WithSummary(supplierTable.Summary)
                .WithSupplierUrl(supplierTable.SupplierUrl)
                .WithAddress(supplierTable.Address)
                .Build()
                .InsertAsync()
                .ConfigureAwait(false);
        }

        private sealed class SupplierTable
        {
            public string Id { get; set; }

            public string SupplierName { get; set; }

            public string Summary { get; set; }

            public string SupplierUrl { get; set; }
            
            public string Address { get; set; }
        }
    }
}
