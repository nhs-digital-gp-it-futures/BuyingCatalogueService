using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.Hostings
{
    public sealed class OnPremiseSection
    {
        public OnPremiseSectionAnswers Answers { get; }

        internal OnPremiseSection(IHosting hosting) =>
            Answers = new OnPremiseSectionAnswers(hosting?.OnPremise);

        public OnPremiseSection IfPopulated() =>
            Answers.HasData ? this : null;
    }
}
