using System.Collections.Generic;

namespace NHSD.BuyingCatalogue.API.ViewModels.Public
{
    public class FeaturesPublicSection
    {
        public FeaturesPublicSectionAnswers Answers { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="FeaturesPublicSection"/> class.
        /// </summary>
        public FeaturesPublicSection(IEnumerable<string> features)
        {
            Answers = new FeaturesPublicSectionAnswers(features);
        }

        public FeaturesPublicSection IfPopulated()
        {
            return Answers.HasData ? this : null;
        }
    }
}
