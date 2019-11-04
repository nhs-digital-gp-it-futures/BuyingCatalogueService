namespace NHSD.BuyingCatalogue.API.ViewModels
{
    public sealed class SolutionDescriptionResult
    {
        public string Summary { get; }

        public string Description { get; }

        public string Link { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="SolutionDescriptionResult"/> class.
        /// </summary>
        public SolutionDescriptionResult(string summary, string description, string link)
        {
            Summary = summary;
            Description = description;
            Link = link;
        }
    }
}
