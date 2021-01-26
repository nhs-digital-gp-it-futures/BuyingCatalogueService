using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using JetBrains.Annotations;
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
                await InsertSupplierAsync(supplier);
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

            var suppliers = await SupplierEntity.FetchAllAsync();
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
            var supplier = await SupplierEntity.GetByIdAsync(supplierId);
            (await supplier.LastUpdated.SecondsFromNow()).Should().BeLessOrEqualTo(5);
        }

        [Given(@"a Supplier (.*) does not exist")]
        public static async Task GivenASolutionSlnDoesNotExist(string supplierId)
        {
            var supplierList = await SupplierEntity.FetchAllAsync();
            supplierList.Select(s => s.Id).Should().NotContain(supplierId);
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
                .InsertAsync();
        }

        [UsedImplicitly(ImplicitUseTargetFlags.Members)]
        private sealed class SupplierTable
        {
            public string Id { get; init; }

            public string SupplierName { get; init; }

            public string Summary { get; init; }

            public string SupplierUrl { get; init; }

            public string Address { get; init; }
        }
    }
}
