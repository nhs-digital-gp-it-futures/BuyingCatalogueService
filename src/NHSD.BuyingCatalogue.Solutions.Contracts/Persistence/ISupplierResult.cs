namespace NHSD.BuyingCatalogue.Solutions.Contracts.Persistence
{
    public interface ISupplierResult
    {
        string Id { get; }

        string Name { get; }

        string AddressLine1 { get; }

        string AddressLine2 { get; }

        string AddressLine3 { get; }

        string AddressLine4 { get; }

        string AddressLine5 { get; }

        string Town { get; }

        string County { get; }

        string Postcode { get; }

        string Country { get; }

        string PrimaryContactFirstName { get; }

        string PrimaryContactLastName { get; }

        string PrimaryContactEmailAddress { get; }

        string PrimaryContactTelephone { get; }

        bool HasAddress { get; }

        bool HasContact { get; }
    }
}
