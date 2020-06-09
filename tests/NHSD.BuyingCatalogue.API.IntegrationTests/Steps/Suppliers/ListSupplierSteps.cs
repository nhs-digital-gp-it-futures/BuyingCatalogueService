using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Flurl;
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

        private readonly ScenarioContext _context;
        private readonly Response _response;

        public ListSupplierSteps(ScenarioContext context, Response response)
        {
            _context = context;
            _response = response;

            _context.Set(new Url(RootSuppliersUrl));
        }

        [Given(@"the user has searched for suppliers matching '(.*)'")]
        public void GivenTheSupplierName(string name)
        {
            GivenTheQueryParameter(nameof(name), name);
        }

        [Given(@"the user has searched for suppliers with solutions matching the publication status '([\w]*)'")]
        public void GivenThePublicationStatus(string solutionPublicationStatus)
        {
            GivenTheQueryParameter(nameof(solutionPublicationStatus), solutionPublicationStatus);
        }

        [When(@"a GET request is made for suppliers")]
        public async Task GetSuppliers()
        {
            var url = _context.Get<Url>();
            _response.Result = await Client.GetAsync(url.ToString());
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

        private void GivenTheQueryParameter<T>(string name, T value)
        {
            var url = _context.Get<Url>();
            url.SetQueryParam(name, value);
        }

        private sealed class ListSuppliersTable
        {
            public string Id { get; set; }

            public string SupplierName { get; set; }
        }
    }
}
