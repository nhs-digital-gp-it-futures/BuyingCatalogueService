using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution
{
    public sealed class ImplementationTimescalesSection
    {
        public ImplementationTimescalesSection(IImplementationTimescales implementationTimescales)
        {
            Answers = new ImplementationTimescalesSectionAnswers(implementationTimescales);
        }

        public ImplementationTimescalesSectionAnswers Answers { get; }

        public ImplementationTimescalesSection IfPopulated() => Answers.HasData ? this : null;
    }
}
