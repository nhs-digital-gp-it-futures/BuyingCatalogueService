namespace NHSD.BuyingCatalogue.Contracts.SolutionList
{
    public interface ISolutionOrganisation
    {
        /// <summary>
        /// Identifier of organisation.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Name of organisation.
        /// </summary>
        string Name { get; }
    }
}
