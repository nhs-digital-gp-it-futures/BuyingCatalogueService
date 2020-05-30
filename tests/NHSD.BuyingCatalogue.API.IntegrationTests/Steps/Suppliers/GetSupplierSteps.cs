using System.Globalization;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Common;
using NHSD.BuyingCatalogue.API.IntegrationTests.Support;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Suppliers
{
    [Binding]
    internal sealed class GetSupplierSteps
    {
        private const string GetSupplierUrlTemplate = "http://localhost:5200/api/v1/suppliers";

        private readonly Response _response;

        public GetSupplierSteps(Response response)
        {
            _response = response;
        }

        [When(@"a GET request is made to retrieve a supplier by ID (.*)")]
        public async Task GetSuppliers(string supplierId) 
            => _response.Result = await Client.GetAsync(GetSupplierUrl(supplierId));

        [Then(@"the response contains the following supplier details")]
        public async Task ThenTheResponseContainsTheFollowingSupplierDetails(Table table)
        {
            var expectedSupplier = table.CreateInstance<GetSupplierTable>();
            var content = await _response.ReadBody();

            var actualSupplier = new GetSupplierTable
            {
                SupplierId = content.Value<string>("supplierId"),
                Name = content.Value<string>("name")
            };

            actualSupplier.Should().BeEquivalentTo(expectedSupplier);
        }

        [Then(@"the response contains the following supplier address details")]
        public async Task ThenTheResponseContainsTheFollowingSupplierAddressDetails(Table table)
        {
            var expectedSupplierAddress  = table.CreateInstance<GetSupplierAddressTable>();
            var content = await _response.ReadBody();

            var addressToken = GetSupplierAddressToken(content);

            var actualSupplierAddress = new GetSupplierAddressTable
            {
                Line1 = addressToken.Value<string>("line1"),
                Line2 = addressToken.Value<string>("line2"),
                Line3 = addressToken.Value<string>("line3"),
                Line4 = addressToken.Value<string>("line4"),
                Line5 = addressToken.Value<string>("line5"),
                Town = addressToken.Value<string>("town"),
                County = addressToken.Value<string>("county"),
                Postcode = addressToken.Value<string>("postcode"),
                Country = addressToken.Value<string>("country")
            };

            actualSupplierAddress.Should().BeEquivalentTo(expectedSupplierAddress);
        }

        [Then(@"the response does not contain a supplier address")]
        public async Task TheTheResponseDoesNotContainASupplierAddress()
        {
            var content = await _response.ReadBody();
            GetSupplierAddressToken(content).Should().BeNull();
        }
        
        [Then(@"the response contains the following supplier primary contact details")]
        public async Task ThenTheResponseContainsTheFollowingSupplierPrimaryContactDetails(Table table)
        {
            var expectedSupplierContact = table.CreateInstance<GetSupplierContactTable>();
            var content = await _response.ReadBody();

            var primaryContactToken = GetSupplierPrimaryContact(content);

            var actualSupplierContact = new GetSupplierContactTable
            {
                FirstName = primaryContactToken.Value<string>("firstName"),
                LastName = primaryContactToken.Value<string>("lastName"),
                EmailAddress = primaryContactToken.Value<string>("emailAddress"),
                TelephoneNumber = primaryContactToken.Value<string>("telephoneNumber")
            };

            actualSupplierContact.Should().BeEquivalentTo(expectedSupplierContact);
        }

        [Then(@"the response does not contain a supplier primary contact")]
        public async Task TheTheResponseDoesNotContainASupplierPrimaryContact()
        {
            var content = await _response.ReadBody();
            GetSupplierPrimaryContact(content).Should().BeNull();
        }

        private static string GetSupplierUrl(string supplierId) 
            => $"{GetSupplierUrlTemplate}/{supplierId}";

        private static JToken GetSupplierAddressToken(JToken content) 
            => content.SelectToken("address");

        private static JToken GetSupplierPrimaryContact(JToken content) 
            => content.SelectToken("primaryContact");

        private sealed class GetSupplierTable
        {
            public string SupplierId { get; set; }

            public string Name { get; set; }
        }

        private sealed class GetSupplierAddressTable
        {
            public string Line1 { get; set; }

            public string Line2 { get; set; }

            public string Line3 { get; set; }

            public string Line4 { get; set; }

            public string Line5 { get; set; }

            public string Town { get; set; }

            public string County { get; set; }

            public string Postcode { get; set; }

            public string Country { get; set; }
        }

        private sealed class GetSupplierContactTable
        {
            public string FirstName { get; set; }

            public string LastName { get; set; }

            public string EmailAddress { get; set; }

            public string TelephoneNumber { get; set; }
        }
    }
}
