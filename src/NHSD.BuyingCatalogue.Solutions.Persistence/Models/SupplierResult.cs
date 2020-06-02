using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Persistence.Models
{
    internal sealed class SupplierResult : ISupplierResult
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string AddressLine3 { get; set; }

        public string AddressLine4 { get; set; }

        public string AddressLine5 { get; set; }

        public string Town { get; set; }

        public string County { get; set; }

        public string Postcode { get; set; }

        public string Country { get; set; }

        public string PrimaryContactFirstName { get; set; }

        public string PrimaryContactLastName { get; set; }

        public string PrimaryContactEmailAddress { get; set; }

        public string PrimaryContactTelephone { get; set; }

        public bool HasAddress { get; set; }

        public bool HasContact { get; set; }
    }
}
