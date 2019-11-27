using System.Collections.Generic;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Preview
{
    public class FeaturesPreviewSection
    {
        public FeaturesPreviewSectionAnswers Answers { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="FeaturesPreviewSection"/> class.
        /// </summary>
        public FeaturesPreviewSection(IEnumerable<string> features)
        {
            Answers = new FeaturesPreviewSectionAnswers(features);
        }

        public FeaturesPreviewSection IfPopulated()
        {
            return Answers.HasData ? this : null;
        }
    }
}
