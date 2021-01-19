using System.Collections.Generic;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution
{
    public sealed class FeaturesSection
    {
        public FeaturesSection(IEnumerable<string> features)
        {
            Answers = new FeaturesSectionAnswers(features);
        }

        public FeaturesSectionAnswers Answers { get; }

        public FeaturesSection IfPopulated()
        {
            return Answers.HasData ? this : null;
        }
    }
}
