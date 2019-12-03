using System.Collections.Generic;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution
{
    public class FeaturesSection
    {
        public FeaturesSectionAnswers Answers { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="FeaturesSection"/> class.
        /// </summary>
        public FeaturesSection(IEnumerable<string> features)
        {
            Answers = new FeaturesSectionAnswers(features);
        }

        public FeaturesSection IfPopulated()
        {
            return Answers.HasData ? this : null;
        }
    }
}
