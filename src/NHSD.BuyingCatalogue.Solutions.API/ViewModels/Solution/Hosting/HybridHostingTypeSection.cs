using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.Hosting
{
    public sealed class HybridHostingTypeSection
    {
        internal HybridHostingTypeSection(IHosting hosting) =>
            Answers = new HybridHostingTypeSectionAnswers(hosting?.HybridHostingType);

        public HybridHostingTypeSectionAnswers Answers { get; }

        public HybridHostingTypeSection IfPopulated() => Answers.HasData ? this : null;
    }
}
