namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.AdditionalService
{
    public sealed class AdditionalServiceResult
    {
        public string AdditionalServiceId { get; set; }

        public string Name { get; set; }

        public string Summary { get; set; }

        public AdditionalServiceSolutionResult Solution { get; set; }
    }
}
