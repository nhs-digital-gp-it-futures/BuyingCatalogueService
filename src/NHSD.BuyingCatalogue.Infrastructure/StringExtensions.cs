namespace NHSD.BuyingCatalogue.Infrastructure
{
    public static class StringExtensions
    {
        public static bool? ToBoolean(this string candidate) => candidate?.ToLower() == "yes" ? true : (candidate?.ToLower() == "no" ? false : (bool?)null);
    }
}
