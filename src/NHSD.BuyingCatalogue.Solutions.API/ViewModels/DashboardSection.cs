using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels
{
    public sealed class DashboardSection
    {
        private readonly bool isRequired;
        private readonly bool isComplete;

        public DashboardSection(bool isRequired, bool isComplete, object section = null)
        {
            this.isRequired = isRequired;
            this.isComplete = isComplete;
            Section = section;
        }

        public string Requirement => isRequired ? "Mandatory" : "Optional";

        public string Status => isComplete ? "COMPLETE" : "INCOMPLETE";

        [JsonProperty("sections")]
        public object Section { get; }

        public static DashboardSection Mandatory(bool isComplete) => new(true, isComplete);

        public static DashboardSection Optional(bool isComplete) => new(false, isComplete);

        public static DashboardSection MandatoryWithSubSection(bool isComplete, object subSection) =>
            new(true, isComplete, subSection);
    }
}
