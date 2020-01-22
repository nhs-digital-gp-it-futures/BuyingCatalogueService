using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.Hostings
{
    public sealed class HybridHostingTypeSection
    {
        public HybridHostingTypeSectionAnswers Answers { get; }

        internal HybridHostingTypeSection(IHosting hosting) =>
            Answers = new HybridHostingTypeSectionAnswers(hosting?.HybridHostingType);

        public HybridHostingTypeSection IfPopulated() =>
            Answers.HasData ? this : null;
    }
}
