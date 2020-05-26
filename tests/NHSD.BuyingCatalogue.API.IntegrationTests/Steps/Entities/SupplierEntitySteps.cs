using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Common;
using NHSD.BuyingCatalogue.API.IntegrationTests.Support;
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
        private const string RootSuppliersUrl = "http://localhost:5200/api/v1/suppliers";

        private readonly Response _response;

        public SupplierEntitySteps(Response response)
        {
            _response = response;
        }

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
                SupplierUrl = string.IsNullOrWhiteSpace(s.SupplierUrl) ? null : s.SupplierUrl
            });

            var suppliers = await SupplierEntity.FetchAllAsync().ConfigureAwait(false);
            suppliers.Select(s => new
            {
                s.Id,
                s.Summary,
                s.SupplierUrl
            }).Should().BeEquivalentTo(expectedSuppliers);
        }

        [Then(@"Last Updated has updated on the Supplier for supplier (.*)")]
        public static async Task LastUpdatedHasUpdatedOnSupplier(string supplierId)
        {
            var supplier = await SupplierEntity.GetByIdAsync(supplierId).ConfigureAwait(false);
            (await supplier.LastUpdated.SecondsFromNow().ConfigureAwait(false)).Should().BeLessOrEqualTo(5);
        }

        [When(@"a GET request is made for suppliers")]
        public async Task GetSuppliers()
        {
            _response.Result = await Client.GetAsync(RootSuppliersUrl);
        }

        [When(@"a GET request is made for suppliers with (\S+) ?(.*)")]
        public async Task GetSuppliersWithName(string field, string value)
        {
            _response.Result = await Client.GetAsync(RootSuppliersUrl, field, value);
        }

        [Then(@"a list of suppliers is returned with the following values")]
        public async Task ThenTheUsersListIsReturnedWithValues(Table table)
        {
            var expectedSuppliers = table.CreateSet<SupplierTable>();
            var content = await _response.ReadBody();
            var actualSuppliers = content.Select(
                t => new SupplierTable
                {
                    Id = t.SelectToken("supplierId").ToString(),
                    SupplierName = t.SelectToken("name").ToString()
                });

            actualSuppliers.Should().BeEquivalentTo(expectedSuppliers);
        }

        private static async Task InsertSupplierAsync(SupplierTable supplierTable)
        {
            await SupplierEntityBuilder.Create()
                .WithId(supplierTable.Id)
                .WithName(supplierTable.SupplierName)
                .WithSummary(supplierTable.Summary)
                .WithSupplierUrl(supplierTable.SupplierUrl)
                .Build()
                .InsertAsync()
                .ConfigureAwait(false);
        }

        private class SupplierTable
        {
            public string Id { get; set; }

            public string SupplierName { get; set; }

            public string Summary { get; set; }

            public string SupplierUrl { get; set; }
        }
    }
}
