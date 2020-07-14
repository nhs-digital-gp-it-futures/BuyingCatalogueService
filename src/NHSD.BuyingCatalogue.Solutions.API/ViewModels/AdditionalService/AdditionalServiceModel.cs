namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.AdditionalService
{
    public sealed class AdditionalServiceModel
    {
        public string AdditionalServiceId { get; set; }

        public string Name { get; set; }

        public string Summary { get; set; }

        public AdditionalServiceSolutionModel Solution { get; set; }
    }
}
