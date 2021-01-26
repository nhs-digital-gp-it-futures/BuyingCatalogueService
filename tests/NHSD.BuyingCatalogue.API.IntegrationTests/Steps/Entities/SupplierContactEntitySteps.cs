using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
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
                await InsertSupplierContactAsync(supplier);
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
                .InsertAsync();
        }

        [UsedImplicitly(ImplicitUseTargetFlags.Members)]
        private sealed class SupplierContactTable
        {
            public Guid Id { get; init; }

            public string SupplierId { get; init; }

            public string FirstName { get; init; }

            public string LastName { get; init; }

            public string Email { get; init; }

            public string PhoneNumber { get; init; }
        }
    }
}
