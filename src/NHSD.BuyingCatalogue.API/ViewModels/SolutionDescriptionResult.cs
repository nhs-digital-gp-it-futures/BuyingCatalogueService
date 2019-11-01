namespace NHSD.BuyingCatalogue.API.ViewModels
{
    public sealed class SolutionDescriptionResult
    {
        public SolutionDescriptionResult(string summary, string description, string link)
        {
            Summary = summary;
            Description = description;
            Link = link;
        }

        public string Summary { get; }

        public string Description { get; }

        public string Link { get; }
    }
}
