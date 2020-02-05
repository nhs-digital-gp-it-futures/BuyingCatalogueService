using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution
{
    public sealed class ImplementationTimescalesSection
    {
        public ImplementationTimescalesSectionAnswers Answers { get; }

        public ImplementationTimescalesSection(IImplementationTimescales implementationTimescales)
        {
            Answers = new ImplementationTimescalesSectionAnswers(implementationTimescales);
        }

        public ImplementationTimescalesSection IfPopulated()
        {
            return Answers.HasData ? this : null;
        }
    }
}
