namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.SubmitForReview
{
    public static class SubmitSolutionForReviewErrors
    {
        public static readonly ValidationError SolutionSummaryIsRequired = new("SolutionSummaryIsRequired");
        public static readonly ValidationError ClientApplicationTypeIsRequired = new("ClientApplicationTypeIsRequired");
        public static readonly ValidationError SupportedBrowserIsRequired = new("SupportedBrowserIsRequired");
        public static readonly ValidationError MobileResponsiveIsRequired = new("MobileResponsiveIsRequired");
        public static readonly ValidationError PluginRequirementIsRequired = new("PluginRequirementIsRequired");
    }
}
