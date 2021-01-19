namespace NHSD.BuyingCatalogue.SolutionLists.Contracts
{
    public interface ISolutionSupplier
    {
        /// <summary>
        /// Gets the ID of supplier.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Gets the name of supplier.
        /// </summary>
        string Name { get; }
    }
}
