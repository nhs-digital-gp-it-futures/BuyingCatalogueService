namespace NHSD.BuyingCatalogue.API.ViewModels
{
    public sealed class SolutionDescriptionResult
    {
        public SolutionDescriptionViewModel SolutionDescription { get; set; }
    }

    public sealed class SolutionDescriptionViewModel
    {
        public string Summary { get; set; }

        public string Description { get; set; }

        public string Link { get; set; }
    }
}
