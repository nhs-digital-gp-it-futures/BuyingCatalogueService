using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Application.Solutions.Commands.SubmitForReview
{
    public abstract class SectionValidationResultViewModel
    {
        [JsonProperty("required")]
        public List<string> MissingQuestions { get; }

        [JsonIgnore]
        public bool HasAnyMissingQuestions => MissingQuestions.Any();

        /// <summary>
        /// Initialises a new instance of the <see cref="SectionValidationResultViewModel"/> class.
        /// </summary>
        public SectionValidationResultViewModel()
        {
            MissingQuestions = new List<string>();
        }

        internal void AddMissingQuestion(string questionId)
        {
            if (string.IsNullOrWhiteSpace(questionId))
            {
                throw new System.ArgumentException("Cannot be null or empty.", nameof(questionId));
            }

            if (!MissingQuestions.Contains(questionId))
            {
                MissingQuestions.Add(questionId);
            }
        }
    }
}
