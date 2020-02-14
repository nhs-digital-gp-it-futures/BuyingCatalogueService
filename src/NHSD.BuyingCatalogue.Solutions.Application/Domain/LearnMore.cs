namespace NHSD.BuyingCatalogue.Solutions.Application.Domain
{
    internal sealed class LearnMore
    {
        internal LearnMore(string documentName) => DocumentName = documentName;

        public string DocumentName { get; }
    }
}
