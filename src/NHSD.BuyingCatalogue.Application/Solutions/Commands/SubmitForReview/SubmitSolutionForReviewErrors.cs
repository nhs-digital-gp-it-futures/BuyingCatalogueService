namespace NHSD.BuyingCatalogue.Application.Solutions.Commands.SubmitForReview
{
    public static class SubmitSolutionForReviewErrors
    {
        public static readonly ValidationError SolutionSummaryIsRequired = new ValidationError("SolutionSummaryIsRequired");
        public static readonly ValidationError ClientApplicationTypeIsRequired = new ValidationError("ClientApplicationTypeIsRequired");
        public static readonly ValidationError SupportedBrowserIsRequired = new ValidationError("SupportedBrowserIsRequired");
        public static readonly ValidationError MobileResponsiveIsRequired = new ValidationError("MobileResponsiveIsRequired");
    }
}
