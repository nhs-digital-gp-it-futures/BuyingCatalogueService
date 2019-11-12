using NHSD.BuyingCatalogue.Infrastructure;

namespace NHSD.BuyingCatalogue.Contracts.Solutions
{
    /// <summary>
    /// Represents the types of statuses a solution can be in related to the suppliers progress.
    /// </summary>
    public sealed class SupplierStatus : Enumeration
    {
        public static readonly SupplierStatus Draft = new SupplierStatus(1, nameof(Draft));
        public static readonly SupplierStatus AuthorityReview = new SupplierStatus(2, nameof(AuthorityReview));

        /// <summary>
        /// Initialises a new instance of the <see cref="SupplierStatus"/> class.
        /// </summary>
        private SupplierStatus(int id, string name) : base(id, name)
        {
        }
    }
}
