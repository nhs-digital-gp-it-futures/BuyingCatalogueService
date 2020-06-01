namespace NHSD.BuyingCatalogue.Solutions.Contracts.Persistence
{
    public interface ISupplierAddress
    {
        string Line1 { get; }

        string Line2 { get; }

        string Line3 { get; }

        string Line4 { get; }

        string Line5 { get; }

        string Town { get; }

        string County { get; }

        string Postcode { get; }

        string Country { get; }
    }
}
