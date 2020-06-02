using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Common;
using NHSD.BuyingCatalogue.API.IntegrationTests.Support;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Suppliers
{
    [Binding]
    internal sealed class ListSupplierSteps
    {
        private const string RootSuppliersUrl = "http://localhost:5200/api/v1/suppliers";

        private readonly Response _response;

        public ListSupplierSteps(Response response)
        {
            _response = response;
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
        public async Task AListOfSuppliersIsReturnedWithTheFollowingValues(Table table)
        {
            var expectedSuppliers = table.CreateSet<ListSuppliersTable>();
            var content = await _response.ReadBody();
            var actualSuppliers = content.Select(
                supplierToken => new ListSuppliersTable
                {
                    Id = supplierToken.SelectToken("supplierId").ToString(),
                    SupplierName = supplierToken.SelectToken("name").ToString()
                });

            actualSuppliers.Should().BeEquivalentTo(expectedSuppliers);
        }

        private sealed class ListSuppliersTable
        {
            public string Id { get; set; }

            public string SupplierName { get; set; }
        }
    }
}
