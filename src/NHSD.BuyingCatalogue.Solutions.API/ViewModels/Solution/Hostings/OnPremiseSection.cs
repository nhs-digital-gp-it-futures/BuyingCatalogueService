using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.Hostings
{
    public sealed class OnPremiseSection
    {
        internal OnPremiseSection(IHosting hosting) => Answers = new OnPremiseSectionAnswers(hosting?.OnPremise);

        public OnPremiseSectionAnswers Answers { get; }

        public OnPremiseSection IfPopulated() => Answers.HasData ? this : null;
    }
}
