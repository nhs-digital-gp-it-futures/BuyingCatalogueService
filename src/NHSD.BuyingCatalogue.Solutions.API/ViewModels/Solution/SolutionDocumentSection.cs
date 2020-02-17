using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution
{
    public sealed class SolutionDocumentSection
    {
        public SolutionDocumentSection(ISolutionDocument solutionDocument) => Answers = new SolutionDocumentSectionAnswers(solutionDocument);

        public SolutionDocumentSectionAnswers Answers { get; }

        internal SolutionDocumentSection IfPopulated() => Answers.HasData ? this : null;
    }
}
