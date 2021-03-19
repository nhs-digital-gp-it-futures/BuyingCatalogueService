namespace NHSD.BuyingCatalogue.Solutions.Contracts
{
    public interface ISolutionFramework
    {
        /// <summary>
        /// Gets the framework Id.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Gets the framework short name.
        /// </summary>
        string ShortName { get; }
    }
}
