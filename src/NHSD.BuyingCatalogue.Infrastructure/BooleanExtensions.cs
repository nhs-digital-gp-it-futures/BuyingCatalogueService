namespace NHSD.BuyingCatalogue.Infrastructure
{
    public static class BooleanExtensions
    {
        public static string ToYesNoString(this bool? candidate) => candidate.HasValue ? candidate.Value ? "Yes" : "No" : null;
    }
}
