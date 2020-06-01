using System;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Common;
using NHSD.BuyingCatalogue.Testing.Data.EntityBuilders;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Entities
{
    [Binding]
    internal sealed class SupplierContactEntitySteps
    {
        [Given(@"Supplier contacts exist")]
        public static async Task GivenSupplierContactsExist(Table table)
        {
            foreach (var supplier in table.CreateSet<SupplierContactTable>())
            {
                await InsertSupplierContactAsync(supplier).ConfigureAwait(false);
            }
        }

        private static async Task InsertSupplierContactAsync(SupplierContactTable supplierTable)
        {
            await SupplierContactEntityBuilder.Create()
                .WithId(supplierTable.Id)
                .WithSupplierId(supplierTable.SupplierId)
                .WithFirstName(supplierTable.FirstName)
                .WithLastName(supplierTable.LastName)
                .WithEmail(supplierTable.Email)
                .WithPhoneNumber(supplierTable.PhoneNumber)
                .Build()
                .InsertAsync()
                .ConfigureAwait(false);
        }

        private sealed class SupplierContactTable
        {
            public Guid Id { get; set; }

            public string SupplierId { get; set; }

            public string FirstName { get; set; }

            public string LastName { get; set; }

            public string Email { get; set; }

            public string PhoneNumber { get; set; }
        }
    }
}
