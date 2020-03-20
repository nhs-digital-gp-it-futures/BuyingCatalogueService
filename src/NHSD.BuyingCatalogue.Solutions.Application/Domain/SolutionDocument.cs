namespace NHSD.BuyingCatalogue.Solutions.Application.Domain
{
    internal sealed class SolutionDocument
    {
        internal SolutionDocument(string documentName) => Name = documentName;

        public string Name { get; }
    }
}
