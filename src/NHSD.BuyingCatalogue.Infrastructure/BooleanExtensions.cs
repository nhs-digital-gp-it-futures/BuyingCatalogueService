namespace NHSD.BuyingCatalogue.Infrastructure
{
    public static class BooleanExtensions
    {
        public static string ToYesNoString(this bool? candidate) =>
            candidate switch
            {
                true => "Yes",
                false => "No",
                _ => null
            };
    }
}
