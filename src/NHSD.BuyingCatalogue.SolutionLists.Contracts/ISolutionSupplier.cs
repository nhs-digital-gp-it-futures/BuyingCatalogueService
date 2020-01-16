namespace NHSD.BuyingCatalogue.SolutionLists.Contracts
{
    public interface ISolutionSupplier
    {
        /// <summary>
        /// Identifier of supplier.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Name of supplier.
        /// </summary>
        string Name { get; }
    }
}
